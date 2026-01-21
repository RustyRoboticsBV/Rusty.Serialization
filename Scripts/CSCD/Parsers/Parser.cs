using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Core.Lexing;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD.Parsing
{
    /// <summary>
    /// A base class for CSCD token parsers.
    /// </summary>
    public class Parser : Core.Parsing.Parser
    {
        /* Fields. */
        private readonly static HashSet<UnicodePair> idEscapes = new HashSet<UnicodePair> { '\t', '\n', ')', '\\' };
        private readonly static HashSet<UnicodePair> typeEscapes = new HashSet<UnicodePair> { '\t', '\n', '`', '\\' };
        private readonly static HashSet<UnicodePair> charEscapes = new HashSet<UnicodePair> { '\t', '\n' };
        private readonly static HashSet<UnicodePair> strEscapes = new HashSet<UnicodePair> { '\t', '\n', '"', '\\' };
        private readonly static HashSet<UnicodePair> refEscapes = new HashSet<UnicodePair> { '\t', '\n', ';', '\\' };
        
        private static readonly Dictionary<char, UnicodePair> simpleEscapes = new()
        {
            { 't', '\t' }, { 'n', '\n' }, { 's', ' ' },
            { '"', '"' }, { '\'', '\'' }, { '`', '`' },
            { '(', '(' }, { ')', ')' },
            { ';', ';' },
            { '\\', '\\' }
        };

        /* Public methods. */
        public override NodeTree Parse(TextSpan text, Lexer lexer)
        {
            INode root = null;
            while (lexer.GetNextToken(text, out Token token))
            {
                if (root != null)
                    throw new FormatException($"Token found after root value: {token.ToString()}.");

                root = ParseToken(text, token, lexer);
            }
            return new NodeTree(root);
        }

        /* Protected methods. */
        protected static INode ParseToken(TextSpan text, Token token, Lexer lexer)
        {
            // Type.
            if (token.Text.StartsWith('(') && token.Text.EndsWith(')'))
            {
                string name = ParseText(token, typeEscapes);

                Token next = ExpectToken(text, lexer, "A type must be followed by another token.");
                INode value = ParseToken(text, next, lexer);

                return new TypeNode(name, value);
            }

            // ID.
            if (token.Text.StartsWith('`') && token.Text.EndsWith('`'))
            {
                string name = ParseText(token, idEscapes);

                Token next = ExpectToken(text, lexer, "An ID must be followed by another token.");
                INode value = ParseToken(text, next, lexer);

                return new IdNode(name, value);
            }

            // Null.
            if (token.Text.Equals("null"))
                return new NullNode();

            // Bool.
            if (token.Text.Equals("true"))
                return new BoolNode(true);
            if (token.Text.Equals("false"))
                return new BoolNode(false);

            // Numerics (int and float).
            NumericType numeric = GetNumericType(token.Text, NumericParseMode.AllowLonePoint);
            if (numeric == NumericType.Int)
                return new IntNode(new string(token.Text));
            if (numeric == NumericType.Real)
                return new FloatNode(ProcessReal(token.Text));

            // NaN.
            if (token.Text.Equals("nan"))
                return new NanNode();

            // Infinity.
            if (token.Text.Equals("inf"))
                return new InfinityNode(true);
            if (token.Text.Equals("-inf"))
                return new InfinityNode(false);

            // Char.
            if (token.Text.StartsWith('\'') && token.Text.EndsWith('\''))
                return new CharNode(ParseText(token, charEscapes));

            // String.
            if (token.Text.StartsWith('"') && token.Text.EndsWith('"'))
                return new StringNode(ParseText(token, strEscapes));

            // Decimal.
            if (token.Text.StartsWith('$') || token.Text.StartsWith("-$"))
                return ParseDecimal(token);

            // Color.
            if (token.Text.StartsWith('#'))
                return new ColorNode(new string(token.Text.Slice(1)));

            // Time.
            if (token.Text.StartsWith('@') && token.Text.EndsWith(';'))
                return ParseDateTime(token);

            // Bytes.
            if (token.Text.StartsWith("b_"))
                return new BytesNode(new string(token.Text.Slice(2)));

            // Ref.
            if (token.Text.StartsWith('&') && token.Text.EndsWith(';'))
                return new RefNode(ParseText(token, refEscapes));

            // List.
            if (token.Text.Equals('['))
                return ParseList(text, lexer);

            // Dictionary.
            if (token.Text.Equals('{'))
                return ParseDictionary(text, lexer);

            // Object.
            if (token.Text.Equals('<'))
                return ParseObject(text, lexer);

            // Illegal tokens.
            TokenError(token, $"Unexpected token.");
            return null;
        }

        /* Private methods. */
        /// <summary>
        /// Parse a decimal number.
        /// </summary>
        private static DecimalNode ParseDecimal(Token token)
        {
            // Get contents.
            bool negative = token.Text.StartsWith('-');
            int offset = negative ? 2 : 1;
            TextSpan contents = token.Text.Slice(offset);

            // Empty literal (integer zero).
            if (contents.Length == 0)
                return new DecimalNode("0");

            // May not have a negative sign after $, or be non-numeric.
            if (contents.StartsWith('-'))
                TokenError(token, "Negative decimals must use the syntax -$ and may not have a - sign after the $ sign.");
            if (GetNumericType(contents, NumericParseMode.AllowLonePoint) == NumericType.NaN)
                TokenError(token, "Non-numeric decimal.");

            // Create proper decimal form (i.e. .5 to 0.5).
            string processed = ProcessReal(contents);

            // Prepend - sign if negative.
            if (negative)
                processed = '-' + processed;

            // Create node.
            return new DecimalNode(processed);
        }

        /// <summary>
        /// Parse a date/time literal.
        /// </summary>
        private static TimeNode ParseDateTime(Token token)
        {
            TextSpan contents = token.Unpack(1, 1);

            // Empty literal.
            if (contents.Length == 0)
                return new TimeNode(1, 1, 1, 0, 0, 0.0);

            TimeNode node = new TimeNode();

            // Date and time.
            int underscore = contents.FirstIndexOf('_');
            if (underscore != -1)
            {
                TextSpan date = contents.Slice(0, underscore);
                ParseDate(node, date);

                TextSpan time = contents.Slice(underscore + 1);
                ParseTime(node, time);

                return node;
            }

            // Date only.
            int dash = contents.FirstIndexOf('-');
            if (dash != -1)
            {
                ParseDate(node, contents);
                return node;
            }

            // Time only.
            int colon = contents.FirstIndexOf(':');
            if (colon != -1)
            {
                ParseTime(node, contents);
                return node;
            }

            TokenError(token, $"Malformed time literal.");
            return null;
        }

        /// <summary>
        /// Parse a sequence of tokens as a list node.
        /// </summary>
        private static ListNode ParseList(TextSpan text, Lexer lexer)
        {

            ListNode list = new ListNode();

            while (true)
            {
                Token next = ExpectToken(text, lexer, "Unclosed list.");

                // Preliminary checks.
                if (list.Count == 0)
                {
                    // Empty list.
                    if (next.Text.Equals(']'))
                        return list;

                    DisallowEqual(next, ',', "Lists may not contain leading commas.");
                }
                else
                {
                    DisallowEqual(next, ',', "Lists may not contain consecutive commas.");
                    DisallowEqual(next, ']', "Lists may not contain trailing commas.");
                }

                // Parse element.
                list.AddValue(ParseToken(text, next, lexer));

                // Next token: comma or list closer.
                next = ExpectToken(text, lexer, "Unclosed list.");

                if (next.Text.Equals(']'))
                    return list;

                MustEqual(next, ',', "List elements must be separated by commas.");
            }
        }

        /// <summary>
        /// Parse a sequence of tokens as a dictionary node.
        /// </summary>
        private static DictNode ParseDictionary(TextSpan text, Lexer lexer)
        {
            DictNode dict = new DictNode();

            while (true)
            {
                Token next = ExpectToken(text, lexer, "Unclosed dictionary.");

                // Preliminary checks.
                if (dict.Count == 0)
                {
                    // Empty dictionary.
                    if (next.Text.Equals('}'))
                        return dict;

                    DisallowEqual(next, ',', "Dictionaries may not contain leading commas.");
                }
                else
                {
                    DisallowEqual(next, ',', "Dictionaries may not contain consecutive commas.");
                    DisallowEqual(next, '}', "Dictionaries may not contain trailing commas.");
                }

                // Parse entry key and value pair.
                INode key = ParseToken(text, next, lexer);

                ExpectSymbol(text, lexer, ':', "Dictionary keys must be followed by a colon.");

                next = ExpectToken(text, lexer, "Unclosed dictionary.");
                DisallowEqual(next, ',', "Dictionary entries must contain a value after the colon.");
                DisallowEqual(next, ':', "Dictionaries may not contain consecutive colons.");
                DisallowEqual(next, '}', "Dictionaries may not contain trailing colons.");
                INode value = ParseToken(text, next, lexer);

                dict.AddPair(key, value);

                // Next token: comma or dictionary closer.
                next = ExpectToken(text, lexer, "Unclosed dictionary.");

                if (next.Text.Equals('}'))
                    return dict;

                MustEqual(next, ',', "Dictionary entries must be separated by commas.");
            }
        }

        /// <summary>
        /// Parse a sequence of tokens as an object node.
        /// </summary>
        private static ObjectNode ParseObject(TextSpan text, Lexer lexer)
        {
            ObjectNode obj = new ObjectNode();

            while (true)
            {
                Token next = ExpectToken(text, lexer, "Unclosed object.");

                // Preliminary checks.
                if (obj.Count == 0)
                {
                    // Empty object.
                    if (next.Text.Equals('>'))
                        return obj;

                    DisallowEqual(next, ',', "Objects may not contain leading commas.");
                }
                else
                {
                    DisallowEqual(next, ',', "Objects may not contain consecutive commas.");
                    DisallowEqual(next, '>', "Objects may not contain trailing commas.");
                }

                // Parse member name & value.
                DisallowEqual(next, ':', "Object names may not equal ':'.");
                string name = next.ToString();

                ExpectSymbol(text, lexer, ':', "Object member names must be followed by a colon.");

                next = ExpectToken(text, lexer, "Unclosed object.");
                DisallowEqual(next, ',', "Object members must contain a value after the colon.");
                DisallowEqual(next, ':', "Objects may not contain consecutive colons.");
                DisallowEqual(next, '>', "Objects may not contain trailing colons.");
                INode valueNode = ParseToken(text, next, lexer);

                obj.AddMember(name, valueNode);

                // Next token: comma or object closer.
                next = ExpectToken(text, lexer, "Unclosed object.");
                if (next.Text.Equals('>'))
                    return obj;

                MustEqual(next, ',', "Object members must be separated by commas.");
            }
        }

        // Helper methods.

        /// <summary>
        /// Parse a textual literal (without the delimiters).
        /// </summary>
        private static string ParseText(Token token, HashSet<UnicodePair> requiredEscapes)
        {
            // Remove enclosing delimiters.
            TextSpan contents = token.Unpack(1, 1);

            StringBuilder sb = new();
            for (int i = 0; i < contents.Length; i++)
            {
                UnicodePair c = contents[i];

                // Illegal characters.
                if (!(c == 0x09 || c == 0x0A || c == 0x0D || c == 0x20
                    || c >= 0x21 && c <= 0x7E
                    || c >= 0xA1 && c <= 0xAC
                    || c >= 0xAE && c <= 0xFF))
                {
                    TokenError(token, $"Illegal character '{c}'.");
                }

                // Escaped.
                else if (ExtractEscape(contents, i, out UnicodePair escaped, out int length))
                {
                    sb.Append(escaped);
                    i += length - 1;
                }

                // Illegal unescaped.
                else if (requiredEscapes.Contains(c))
                    TokenError(token, $"Illegal unescaped character '{c}'.");

                // Non-escaped.
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Parse an escape sequence (and get the original sequence's length).
        /// </summary>
        private static bool ExtractEscape(TextSpan span, int index, out UnicodePair chr, out int sequenceLength)
        {
            // Must start with a backslash.
            char current = span[index];
            if (current != '\\')
            {
                chr = 0;
                sequenceLength = 0;
                return false;
            }

            // Must be followed by another character.
            if (index + 1 >= span.Length)
                throw new FormatException($"Unclosed escape sequence at {new string(span)}.");

            char next = span[index + 1];

            // Simple escape codes.
            if (simpleEscapes.TryGetValue(next, out chr))
            {
                sequenceLength = 2;
                return true;
            }

            // Unicode.
            else
            {
                int unicodeEnd = span.FirstIndexOf(index + 1, ';');
                if (unicodeEnd == -1)
                    throw new FormatException($"Unclosed unicode escape sequence at {new string(span)}.");

                int hexLength = unicodeEnd - (index + 1);
                TextSpan unicodeHex = span.Slice(index + 1, hexLength);
                try
                {
                    chr = ParseHex(unicodeHex);
                    sequenceLength = hexLength + 2;
                }
                catch
                {
                    throw new FormatException($"Invalid unicode escape sequence at {new string(span)}.");
                }
            }

            return true;
        }

        /// <summary>
        /// Process a real string to make sure there is at least 1 digit before AND after the decimal point.
        /// We handle: . as 0.0, -. as -0.0, .x as 0.x, -.x as -0.x, x. as x.0 and -x. as -x.0.
        /// Does not affect integers or fully-formed real numbers.
        /// </summary>
        private static string ProcessReal(TextSpan span)
        {
            // Handle lone .
            if (span.Equals('.'))
                return "0.0";

            // Handle -.
            if (span.Equals("-."))
                return "-0.0";

            // Handle .x
            else if (span.StartsWith("."))
                return "0." + new string(span.Slice(1));

            // Handle -.x
            else if (span.StartsWith("-."))
                return "-0." + new string(span.Slice(2));

            // Handle x. and -x.
            else if (span.EndsWith("."))
                return new string(span) + '0';

            // No change needed.
            else
                return new string(span);
        }

        /// <summary>
        /// Parse a date component.
        /// </summary>
        private static void ParseDate(TimeNode node, TextSpan date)
        {
            // Negative year.
            bool negativeYear = false;
            if (date.StartsWith('-'))
            {
                negativeYear = true;
                date = date.Slice(1);
            }

            // Parse year.
            int endOfYear = date.FirstIndexOf('-');
            if (endOfYear == -1)
                throw new FormatException($"Date does not contain a year: {new string(date)}.");

            TextSpan year = date.Slice(0, endOfYear);
            if (year.Length == 0)
                throw new FormatException($"Empty year term in: {new string(date)}.");
            if (year.StartsWith('-'))
                throw new FormatException($"Duplicate minus sign in: {new string(date)}.");
            if (GetNumericType(year) != NumericType.Int)
                throw new FormatException($"Not an integer year: {new string(date)}."); 

            if (negativeYear)
                node.Year = '-' + new string(year);
            else
                node.Year = new string(year);

            // Parse month.
            int endOfMonth = date.FirstIndexOf(endOfYear + 1, '-');
            if (endOfMonth == -1)
                throw new FormatException($"Date does not contain a month: {new string(date)}.");

            TextSpan month = date.Slice(endOfYear + 1, endOfMonth - (endOfYear + 1));
            if (month.Length == 0)
                throw new FormatException($"Empty month term in: {new string(date)}.");
            if (month.StartsWith('-'))
                throw new FormatException($"Negative month in: {new string(date)}.");
            if (GetNumericType(month) != NumericType.Int)
                throw new FormatException($"Not an integer month: {new string(date)}.");
            if (!IsWithinRange(month, 1, 12))
                throw new FormatException($"Month not in the range [1-12]: {new string(date)}.");

            node.Month = new string(month);

            // Parse day.
            TextSpan day = date.Slice(endOfMonth + 1);
            if (day.Length == 0)
                throw new FormatException($"Empty day term in: {new string(date)}.");
            if (day.StartsWith('-'))
                throw new FormatException($"Negative day in: {new string(date)}.");
            if (GetNumericType(day) != NumericType.Int)
                throw new FormatException($"Not an integer day: {new string(date)}.");
            if (!IsWithinRange(day, 1, 31))
                throw new FormatException($"Day not in the range [1-31]: {new string(date)}.");

            node.Day = new string(day);
        }

        /// <summary>
        /// Parse a time component.
        /// </summary>
        private static void ParseTime(TimeNode node, TextSpan time)
        {
            // Parse hour.
            int endOfHour = time.FirstIndexOf(':');
            if (endOfHour == -1)
                throw new FormatException($"Time does not contain an hour: {new string(time)}.");

            TextSpan hour = time.Slice(0, endOfHour);
            if (hour.Length == 0)
                throw new FormatException($"Empty hour term in: {new string(time)}.");
            if (hour.StartsWith('-'))
                throw new FormatException($"Negative hour in: {new string(time)}.");
            if (GetNumericType(hour) != NumericType.Int)
                throw new FormatException($"Not an integer hour: {new string(time)}.");
            if (!IsWithinRange(hour, 0, 24))
                throw new FormatException($"Hour not in the range [0-24]: {new string(time)}.");

            node.Hour = new string(hour);

            // Parse minute.
            int endOfMinute = time.FirstIndexOf(endOfHour + 1, ':');
            if (endOfMinute == -1)
                throw new FormatException($"Time does not contain a minute: {new string(time)}.");

            TextSpan minute = time.Slice(endOfHour + 1, endOfMinute - (endOfHour + 1));
            if (minute.Length == 0)
                throw new FormatException($"Empty minute term in: {new string(time)}.");
            if (minute.StartsWith('-'))
                throw new FormatException($"Negative minute in: {new string(time)}.");
            if (GetNumericType(minute) != NumericType.Int)
                throw new FormatException($"Not an integer minute: {new string(time)}.");
            if (!IsWithinRange(minute, 0, 59))
                throw new FormatException($"Hour not in the range [0-59]: {new string(time)}.");

            node.Minute = new string(minute);

            // Parse second.
            TextSpan second = time.Slice(endOfMinute + 1);
            if (second.Length == 0)
                throw new FormatException($"Empty second term in: {new string(time)}.");
            if (second.StartsWith('-'))
                throw new FormatException($"Negative second in: {new string(time)}.");
            if (GetNumericType(second, NumericParseMode.AllowLonePoint) == NumericType.NaN)
                throw new FormatException($"Not a numeric second: {new string(time)}.");
            int pointIndex = second.FirstIndexOf('.');
            TextSpan integerPart = pointIndex == -1 ? second : second.Slice(0, pointIndex);
            if (!IsWithinRange(integerPart, 0, 60))
                throw new FormatException($"Second not in the range [0-60]: {new string(time)}.");

            node.Second = new string(second);
        }
    }
}