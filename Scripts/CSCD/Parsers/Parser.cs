using System;
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
        private readonly static char[] idEscapes = new char[] { '\t', '\n', ')', '\\' };
        private readonly static char[] typeEscapes = new char[] { '\t', '\n', '`', '\\' };
        private readonly static char[] charEscapes = new char[] { '\t', '\n' };
        private readonly static char[] strEscapes = new char[] { '\t', '\n', '"', '\\' };
        private readonly static char[] refEscapes = new char[] { '\t', '\n', ';', '\\' };

        /* Public methods. */
        public override NodeTree Parse(TextSpan text, Lexer lexer)
        {
            INode root = null;
            while (lexer.GetNextToken(text, out Token token))
            {
                if (root != null)
                    throw new FormatException($"Token found after root value: {token.ToString()}.");

                root = ParseToken(text, token, ref lexer);
            }
            return new NodeTree(root);
        }

        /* Protected methods. */
        protected static INode ParseToken(TextSpan text, Token token, ref Lexer lexer)
        {
            // Type.
            if (token.Text.StartsWith('(') && token.Text.EndsWith(')'))
            {
                string name = ParseText(token.Text.Slice(1, token.Text.Length - 2), typeEscapes);

                if (!lexer.GetNextToken(text, out Token next))
                    throw new FormatException("A type must be followed by another token.");
                INode value = ParseToken(text, next, ref lexer);

                return new TypeNode(name, value);
            }

            // ID.
            if (token.Text.StartsWith('`') && token.Text.EndsWith('`'))
            {
                string name = ParseText(token.Text.Slice(1, token.Text.Length - 2), idEscapes);

                if (!lexer.GetNextToken(text, out Token next))
                    throw new FormatException("An ID must be followed by another.");
                INode value = ParseToken(text, next, ref lexer);

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
            int numeric = GetNumeric(token.Text);
            if (numeric == 0)
                return new IntNode(new string(token.Text));
            if (numeric == 1)
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
            {
                string str = ParseText(token.Text.Slice(1, token.Text.Length - 2), charEscapes);
                return new CharNode(str);
            }

            // String.
            if (token.Text.StartsWith('"') && token.Text.EndsWith('"'))
            {
                string str = ParseText(token.Text.Slice(1, token.Text.Length - 2), strEscapes);
                return new StringNode(str);
            }

            // Decimal.
            if (token.Text.StartsWith('$'))
                return new DecimalNode(ParseDecimal(token.Text, 1));
            if (token.Text.StartsWith("-$"))
                return new DecimalNode('-' + ParseDecimal(token.Text, 2));

            // Color.
            if (token.Text.StartsWith('#'))
                return new ColorNode(new string(token.Text.Slice(1)));

            // Time.
            if (token.Text.StartsWith('@') && token.Text.EndsWith(';'))
                return ParseDateTime(token.Text);

            // Bytes.
            if (token.Text.StartsWith("b_"))
                return new BytesNode(new string(token.Text.Slice(2)));

            // Ref.
            if (token.Text.StartsWith('&') && token.Text.EndsWith(';'))
            {
                string str = ParseText(token.Text.Slice(1, token.Text.Length - 2), refEscapes);
                return new RefNode(str);
            }

            // List.
            if (token.Text.Equals('['))
            {
                ListNode list = new ListNode();

                while (true)
                {
                    if (!lexer.GetNextToken(text, out Token next))
                        throw new FormatException("Unclosed list.");
                    TextSpan nextText = next.Text;

                    // List closer.
                    if (nextText.Equals(']'))
                        return list;

                    // First element.
                    if (list.Count == 0)
                    {
                        if (nextText.Equals(','))
                            throw new FormatException("Leading commas are not allowed.");
                        list.AddValue(ParseToken(text, next, ref lexer));
                    }

                    // Second element onwards.
                    else
                    {
                        // Elements must be separated with a comma.
                        if (!nextText.Equals(','))
                            throw new FormatException("List elements must be separated with commas.");

                        // There must be a value token.
                        if (!lexer.GetNextToken(text, out Token valueToken))
                            throw new FormatException("Unclosed list.");

                        // May not close a list after a comma.
                        if (valueToken.Text.Equals(']'))
                            throw new FormatException("Trailing commas are not allowed.");

                        // May not be another comma.
                        if (valueToken.Text.Equals(','))
                            throw new FormatException("Consecutive commas are not allowed.");

                        // Parse element.
                        list.AddValue(ParseToken(text, valueToken, ref lexer));
                    }
                }
            }

            // Dict.
            if (token.Text.Equals('{'))
            {
                DictNode dict = new DictNode();

                while (true)
                {
                    if (!lexer.GetNextToken(text, out Token next))
                        throw new FormatException("Unclosed dictionary.");

                    TextSpan nextText = next.Text;

                    // Dictionary closer.
                    if (nextText.Equals('}'))
                        return dict;

                    // First pair.
                    if (dict.Count == 0)
                    {
                        if (nextText.Equals(','))
                            throw new FormatException("Leading commas are not allowed in dictionaries.");

                        // Parse key.
                        INode key = ParseToken(text, next, ref lexer);

                        // Expect colon.
                        if (!lexer.GetNextToken(text, out Token colon) || !colon.Text.Equals(':'))
                            throw new FormatException("Dictionary keys must be followed by a colon.");

                        // Parse value.
                        if (!lexer.GetNextToken(text, out Token valueToken))
                            throw new FormatException("Unclosed dictionary.");

                        INode value = ParseToken(text, valueToken, ref lexer);
                        dict.AddPair(key, value);
                    }
                    // Subsequent pairs.
                    else
                    {
                        // Must be comma.
                        if (!nextText.Equals(','))
                            throw new FormatException("Dictionary entries must be separated with commas.");

                        // Expect key.
                        if (!lexer.GetNextToken(text, out Token keyToken))
                            throw new FormatException("Unclosed dictionary.");

                        if (keyToken.Text.Equals('}'))
                            throw new FormatException("Trailing commas are not allowed in dictionaries.");

                        if (keyToken.Text.Equals(','))
                            throw new FormatException("Consecutive commas are not allowed in dictionaries.");

                        INode key = ParseToken(text, keyToken, ref lexer);

                        // Expect colon.
                        if (!lexer.GetNextToken(text, out Token colon) || !colon.Text.Equals(':'))
                            throw new FormatException("Dictionary keys must be followed by a colon.");

                        // Expect value.
                        if (!lexer.GetNextToken(text, out Token valueToken))
                            throw new FormatException("Unclosed dictionary.");

                        INode value = ParseToken(text, valueToken, ref lexer);
                        dict.AddPair(key, value);
                    }
                }
            }


            // Object.
            if (token.Text.Equals('<'))
            {
                ObjectNode obj = new ObjectNode();

                while (true)
                {
                    if (!lexer.GetNextToken(text, out Token next))
                        throw new FormatException("Unclosed object.");

                    TextSpan nextText = next.Text;

                    // Object closer.
                    if (nextText.Equals('>'))
                        return obj;

                    // First field.
                    if (obj.Count == 0)
                    {
                        if (nextText.Equals(','))
                            throw new FormatException("Leading commas are not allowed in objects.");

                        // Key is literal token text (NOT parsed).
                        string key = new string(nextText);

                        // Expect colon.
                        if (!lexer.GetNextToken(text, out Token colon) || !colon.Text.Equals(':'))
                            throw new FormatException("Object fields must be in the form <key:value>.");

                        // Expect value.
                        if (!lexer.GetNextToken(text, out Token valueToken))
                            throw new FormatException("Unclosed object.");

                        INode value = ParseToken(text, valueToken, ref lexer);
                        obj.AddMember(key, value);
                    }
                    // Subsequent fields.
                    else
                    {
                        // Must be comma.
                        if (!nextText.Equals(','))
                            throw new FormatException("Object fields must be separated with commas.");

                        // Expect key.
                        if (!lexer.GetNextToken(text, out Token keyToken))
                            throw new FormatException("Unclosed object.");

                        if (keyToken.Text.Equals('>'))
                            throw new FormatException("Trailing commas are not allowed in objects.");

                        if (keyToken.Text.Equals(','))
                            throw new FormatException("Consecutive commas are not allowed in objects.");

                        string key = keyToken.ToString();

                        // Expect colon.
                        if (!lexer.GetNextToken(text, out Token colon) || !colon.Text.Equals(':'))
                            throw new FormatException("Object fields must be in the form <key:value>.");

                        // Expect value.
                        if (!lexer.GetNextToken(text, out Token valueToken))
                            throw new FormatException("Unclosed object.");

                        INode value = ParseToken(text, valueToken, ref lexer);
                        obj.AddMember(key, value);
                    }
                }
            }


            // Illegal tokens.
            throw new FormatException($"Illegal token: {new string(token.Text)}.");
        }

        /// <summary>
        /// Parse a textual literal (without the delimiters).
        /// </summary>
        private static string ParseText(TextSpan span, char[] requiredEscapes)
        {
            StringBuilder sb = new();
            for (int i = 0; i < span.Length; i++)
            {
                char c = span[i];

                // Illegal characters.
                if (!(c == 0x09 || c == 0x0A || c == 0x0D || c == 0x20
                    || c >= 0x21 && c <= 0x7E
                    || c >= 0xA1 && c <= 0xAC
                    || c >= 0xAE && c <= 0xFF))
                {
                    throw new FormatException($"Illegal character '{(int)c:X}' at {new string(span)}.");
                }

                // Escaped.
                else if (ExtractEscape(span, i, out UnicodePair escaped, out int length))
                {
                    sb.Append(escaped);
                    i += length - 1;
                }

                // Illegal unescaped.
                else if (IsRequiredEscaped(c, requiredEscapes))
                    throw new FormatException($"Illegal unescaped character '{(int)c:X}' at {new string(span)}.");

                // Non-escaped.
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Check if a character should be escaped.
        /// </summary>
        private static bool IsRequiredEscaped(char chr, char[] requiredEscapes)
        {
            for (int i = 0; i < requiredEscapes.Length; i++)
            {
                if (chr == requiredEscapes[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Parse an escape sequence (and get the original sequence's length).
        /// </summary>
        private static bool ExtractEscape(TextSpan span, int index, out UnicodePair chr, out int length)
        {
            // Must start with a backslash.
            char current = span[index];
            if (current != '\\')
            {
                chr = 0;
                length = 0;
                return false;
            }

            // Must be followed by another character.
            if (index + 1 >= span.Length)
                throw new FormatException($"Unclosed escape sequence at {new string(span)}.");

            char next = span[index + 1];

            // Whitespace.
            if (next == 't')
            {
                chr = '\t';
                length = 2;
            }
            else if (next == 'n')
            {
                chr = '\n';
                length = 2;
            }
            else if (next == 's')
            {
                chr = ' ';
                length = 2;
            }

            // Delimiter characters.
            else if (next == '"' || next == '\'' || next == '(' || next == ')'
                || next == '\\' || next == ';' || next == '`')
            {
                chr = next;
                length = 2;
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
                    length = hexLength + 2;
                }
                catch
                {
                    throw new FormatException($"Invalid unicode escape sequence at {new string(span)}.");
                }
            }

            return true;
        }

        /// <summary>
        /// Parse a hex number.
        /// </summary>
        private static int ParseHex(ReadOnlySpan<char> hex)
        {
            if (hex.Length == 0)
                throw new FormatException("Input cannot be empty.");

            int result = 0;

            for (int i = 0; i < hex.Length; i++)
            {
                char c = hex[i];
                int value;

                if (c >= '0' && c <= '9')
                    value = c - '0';
                else if (c >= 'A' && c <= 'F')
                    value = c - 'A' + 10;
                else if (c >= 'a' && c <= 'f')
                    value = c - 'a' + 10;
                else
                    throw new FormatException($"Invalid hex character: '{c}'");

                result = (result << 4) | value;
            }

            return result;
        }

        /// <summary>
        /// Parse a decimal number.
        /// </summary>
        private static string ParseDecimal(TextSpan span, int offset)
        {
            TextSpan contents = span.Slice(offset);

            // Empty literal (integer zero).
            if (contents.Length == 0)
                return "0";

            // May not have a negative sign after $, or be non-numeric.
            if (contents.StartsWith('-'))
                throw new FormatException($"Malformed decimal token: {new string(span)}.");
            if (GetNumeric(contents) == -1)
                throw new FormatException($"Non-numeric decimal token: {new string(span)}.");

            // Create [decimal.[fractional] form.
            return ProcessReal(contents);
        }

        /// <summary>
        /// Parse a date/time literal.
        /// </summary>
        private static TimeNode ParseDateTime(TextSpan span)
        {
            TextSpan contents = span.Slice(1, span.Length - 2);

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

            throw new FormatException($"Malformed time literal: {new string(span)}.");
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
            if (GetNumeric(year) != 0)
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
            if (GetNumeric(month) != 0)
                throw new FormatException($"Not an integer month: {new string(date)}.");

            node.Month = new string(month);

            // Parse day.
            TextSpan day = date.Slice(endOfMonth + 1);
            if (day.Length == 0)
                throw new FormatException($"Empty day term in: {new string(date)}.");
            if (day.StartsWith('-'))
                throw new FormatException($"Negative day in: {new string(date)}.");
            if (GetNumeric(day) != 0)
                throw new FormatException($"Not an integer day: {new string(date)}.");

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
            if (GetNumeric(hour) != 0)
                throw new FormatException($"Not an integer hour: {new string(time)}.");

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
            if (GetNumeric(minute) != 0)
                throw new FormatException($"Not an integer minute: {new string(time)}.");

            node.Minute = new string(minute);

            // Parse second.
            TextSpan second = time.Slice(endOfMinute + 1);
            if (second.Length == 0)
                throw new FormatException($"Empty second term in: {new string(time)}.");
            if (second.StartsWith('-'))
                throw new FormatException($"Negative second in: {new string(time)}.");
            if (GetNumeric(second) == -1)
                throw new FormatException($"Not a numeric second: {new string(time)}.");

            node.Second = new string(second);
        }

        /// <summary>
        /// Returns -1 if not numeric, 0 if integer and 1 if float.
        /// Interprets special literals . and -. as 0.0 and -0.0, respectively.
        /// </summary>
        private static int GetNumeric(TextSpan span)
        {
            int i = 0;
            bool fractional = false;

            // Leading minus sign.
            if (span[0] == '-')
            {
                if (span.Length == 1)
                    return -1;
                else
                    i++;
            }

            // Check remaining characters.
            for (; i < span.Length; i++)
            {
                // Decimal point.
                if (span[i] == '.')
                {
                    if (!fractional)
                        fractional = true;
                    else
                        return -1;
                }

                // Not numeric.
                else if (span[i] < '0' || span[i] > '9')
                    return -1;
            }
            if (fractional)
                return 1;
            else
                return 0;
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
                return "0." + new string(span.Slice(2));

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
    }
}