using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A base class for CSCD token parsers.
    /// </summary>
    public class CscdParser : Parser<CscdLexer>
    {
        /* Fields. */
        private readonly static HashSet<UnicodePair> idEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '`', '\\' };
        private readonly static HashSet<UnicodePair> typeEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', ')', '\\' };
        private readonly static HashSet<UnicodePair> charEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r' };
        private readonly static HashSet<UnicodePair> strEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '"', '\\' };
        private readonly static HashSet<UnicodePair> symbolEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '*', '\\' };
        private readonly static HashSet<UnicodePair> refEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '&', '\\' };

        private static readonly Dictionary<char, UnicodePair> simpleEscapes = new Dictionary<char, UnicodePair>
        {
            { 't', '\t' },
            { 'n', '\n' },
            { 's', ' ' },
            { '"', '"' },
            { '&', '&' },
            { '\'', '\'' },
            { '(', '(' },
            { ')', ')' },
            { '*', '*' },
            { '\\', '\\' },
            { '`', '`' }
        };

        /* Public methods. */
        public override NodeTree Parse(TextSpan text, CscdLexer lexer)
        {
            INode root = null;

            // Skip format marker (if present).
            lexer.GetNextToken(text, out Token marker);
            if (!marker.Text.Equals("~CSCD~"))
                lexer.ResetCursor();

            // Root.
            while (lexer.GetNextToken(text, out Token token))
            {
                if (root != null)
                    throw new FormatException($"Token found after root value: {token.ToString()}.");

                root = ParseToken(text, token, lexer);
            }

            // Ensure legal root value.
            INode check = root;
            if (check is IdNode id)
                check = id.Value;
            if (check is TypeNode type)
                check = type.Value;
            if (check is RefNode)
                throw new FormatException("Root values may not be references.");
            if (check == null)
                throw new FormatException("No root value.");

            // Create tree.
            return new NodeTree(root);
        }

        /* Protected methods. */
        protected static INode ParseToken(TextSpan text, Token token, CscdLexer lexer)
        {
            // Format marker.
            if (token.Text.Equals("~CSCD~"))
                TokenError(token, "Format marker may not appear after first token.");

            // Type.
            if (token.Text.StartsWith('(') && token.Text.EndsWith(')'))
            {
                string name = ParseText(token, typeEscapes, "(", ")");

                Token next = ExpectToken(text, lexer, "A type must be followed by another token.");
                INode value = ParseToken(text, next, lexer);
                if (value is IdNode)
                    TokenError(token, "Types may not be followed by an ID.");
                if (value is TypeNode)
                    TokenError(token, "Types may not be followed by a type.");

                return new TypeNode(name, value);
            }

            // ID.
            if (token.Text.StartsWith('`') && token.Text.EndsWith('`'))
            {
                string name = ParseText(token, idEscapes, "`", "`");

                Token next = ExpectToken(text, lexer, "An ID must be followed by another token.");
                INode value = ParseToken(text, next, lexer);
                if (value is IdNode)
                    TokenError(token, "IDs may not be followed by another ID.");

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
                return new IntNode(IntValue.Parse(token.Text));
            if (numeric == NumericType.Real)
                return new FloatNode(FloatValue.Parse(ProcessReal(token.Text)));

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
                return ParseChar(token);

            // String.
            if (token.Text.StartsWith('"') && token.Text.EndsWith('"'))
                return new StringNode(ParseText(token, strEscapes, "\"", "\""));

            // Decimal.
            if (token.Text.StartsWith('$') || token.Text.StartsWith("-$"))
                return ParseDecimal(token);

            // Color.
            if (token.Text.StartsWith('#'))
                return ParseColor(token);

            // Time.
            if (token.Text.StartsWith('@') && token.Text.EndsWith('@'))
                return ParseTimestamp(token);

            // Bytes.
            if (token.Text.StartsWith('!'))
                return ParseBytes(token);

            // Symbol (delimited).
            if (token.Text.StartsWith('*') && token.Text.EndsWith('*'))
                return new SymbolNode(ParseText(token, symbolEscapes, "*", "*"));

            // Ref.
            if (token.Text.StartsWith('&') && token.Text.EndsWith('&'))
                return new RefNode(ParseText(token, refEscapes, "&", "&"));

            // List.
            if (token.Text.Equals('['))
                return ParseList(text, lexer);

            // Dictionary.
            if (token.Text.Equals('{'))
                return ParseDictionary(text, lexer);

            // Object.
            if (token.Text.Equals('<'))
                return ParseObject(text, lexer);

            // Symbol (bare).
            return ParseBareSymbol(token, false);
        }

        /* Private methods. */
        /// <summary>
        /// Parse a char literal.
        /// </summary>
        private static CharNode ParseChar(Token token)
        {
            string str = ParseText(token, charEscapes, "'", "'");
            if (str.Length < 1 || str.Length > 2 || (str.Length == 2 && !char.IsHighSurrogate(str[0])))
                TokenError(token, "Char token may not represent multiple characters.");
            return new CharNode(str);
        }

        /// <summary>
        /// Parse a decimal number.
        /// </summary>
        private static DecimalNode ParseDecimal(Token token)
        {
            if (!token.Text.StartsWith('$') && !token.Text.StartsWith("-$"))
                TokenError(token, "Decimal tokens must start with a $ or -$ prefix.");

            // Get contents.
            bool negative = token.Text.StartsWith('-');
            int offset = negative ? 2 : 1;
            TextSpan contents = token.Text.Slice(offset);

            // Empty literal (integer zero).
            if (contents.Length == 0)
                return new DecimalNode(0m);

            // May not have a negative sign after $, or be non-numeric.
            if (contents.StartsWith('-'))
                TokenError(token, "Decimals may not have a - sign after the $ sign; use -$ instead.");
            if (GetNumericType(contents, NumericParseMode.AllowLonePoint) == NumericType.NaN)
                TokenError(token, "Non-numeric decimal.");

            // Create proper decimal form (i.e. .5 to 0.5).
            string processed = ProcessReal(contents);

            // Prepend - sign if negative.
            if (negative)
                processed = '-' + processed;

            // Create node.
            return new DecimalNode(DecimalValue.Parse(processed));
        }

        /// <summary>
        /// Parse a color literal.
        /// </summary>
        private static ColorNode ParseColor(Token token)
        {
            if (!token.Text.StartsWith('#'))
                TokenError(token, "Missing # prefix.");

            if (token.Text.Equals('#'))
                return new ColorNode(new ColorValue(0, 0, 0, 0));

            try
            {
                return new ColorNode(ColorValue.Parse(token.Text));
            }
            catch (Exception ex)
            {
                TokenError(token, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Parse a date/time literal.
        /// </summary>
        private static TimeNode ParseTimestamp(Token token)
        {
            TextSpan contents = token.Unpack(1, 1);

            IntValue year = 1;
            IntValue month = 1;
            IntValue day = 1;
            IntValue hour = 0;
            IntValue minute = 0;
            FloatValue second = 0f;

            try
            {
                // Empty literal.
                if (contents.Length == 0)
                    return new TimeNode(year, month, day, hour, minute, second);

                // Date and time.
                int underscore = contents.FirstIndexOf(',');
                if (underscore != -1)
                {
                    TextSpan date = contents.Slice(0, underscore);
                    ParseDate(date, out year, out month, out day);

                    TextSpan time = contents.Slice(underscore + 1);
                    ParseTime(time, out hour, out minute, out second);

                    goto Return;
                }

                // Date only.
                int dash = contents.FirstIndexOf('/');
                if (dash != -1)
                {
                    ParseDate(contents, out year, out month, out day);
                    goto Return;
                }

                // Time only.
                int colon = contents.FirstIndexOf(':');
                if (colon != -1)
                {
                    ParseTime(contents, out hour, out minute, out second);
                    goto Return;
                }
            }
            catch (Exception ex)
            {
                TokenError(token, ex.Message);
                return null;
            }

            TokenError(token, "Malformed time literal.");
            return null;

            Return: return new TimeNode(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Parse a bytes literal.
        /// </summary>
        private static BytesNode ParseBytes(Token token)
        {
            if (!token.Text.StartsWith('!'))
                TokenError(token, "Missing ! prefix.");

            // Get Base64 contents.
            TextSpan contents = token.Text.Slice("!".Length);

            // Add padding if needed.
            int paddedLength = contents.Length % 4 != 0 ? (contents.Length / 4 + 1) * 4 : contents.Length;
            Span<char> span = stackalloc char[paddedLength];
            contents.AsSpan().CopyTo(span);
            for (int i = contents.Length; i < paddedLength; i++)
            {
                span[i] = '=';
            }

            // Create node.
            return new BytesNode(BytesValue.Parse(span));
        }

        /// <summary>
        /// Parse a symbol literal.
        /// </summary>
        private static SymbolNode ParseSymbol(Token token, bool allowReservedKeywords)
        {
            if (token.Text.StartsWith('*') && token.Text.EndsWith('*'))
                return new SymbolNode(ParseText(token, symbolEscapes, "*", "*"));
            else
                return ParseBareSymbol(token, allowReservedKeywords);
        }

        /// <summary>
        /// Parse a sequence of tokens as a list node.
        /// </summary>
        private static ListNode ParseList(TextSpan text, CscdLexer lexer)
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
        private static DictNode ParseDictionary(TextSpan text, CscdLexer lexer)
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
        private static ObjectNode ParseObject(TextSpan text, CscdLexer lexer)
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
                SymbolNode name = ParseSymbol(next, true);

                ExpectSymbol(text, lexer, ':', "Object member names must be followed by a colon.");

                next = ExpectToken(text, lexer, "Unclosed object.");
                DisallowEqual(next, ',', "Object members must contain a value after the colon.");
                DisallowEqual(next, ':', "Objects may not contain consecutive colons.");
                DisallowEqual(next, '>', "Objects may not contain trailing colons.");
                INode valueNode = ParseToken(text, next, lexer);

                obj.AddMember(name.Name, valueNode);

                // Next token: comma or object closer.
                next = ExpectToken(text, lexer, "Unclosed object.");
                if (next.Text.Equals('>'))
                    return obj;

                MustEqual(next, ',', "Object members must be separated by commas.");
            }
        }

        /// <summary>
        /// Parse a sequence of tokens as an object node.
        /// </summary>
        private static SymbolNode ParseBareSymbol(Token token, bool allowReservedKeywords)
        {
            if (token.Length == 0)
                TokenError(token, "Bare tokens may not be empty.");

            if (!allowReservedKeywords)
            {
                if (token.Text.Equals("null") || token.Text.Equals("true") || token.Text.Equals("false")
                    || token.Text.Equals("nan") || token.Text.Equals("inf"))
                {
                    TokenError(token, "Symbol may not be a reserved keyword.");
                }
            }

            char c = token.Text[0];
            if (!(c == '_' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' ))
                TokenError(token, "Bare tokens must starts with an ASCII letter or underscore.");

            for (int i = 1; i < token.Length; i++)
            {
                c = token.Text[i];
                if (!(c == '_' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9'))
                    TokenError(token, "Bare tokens may only consist of ASCII letters, digits or underscores.");
            }

            return new SymbolNode(new string(token.Text));
        }

        // Helper methods.

        /// <summary>
        /// Parse a textual literal (without the delimiters).
        /// </summary>
        private static string ParseText(Token token, HashSet<UnicodePair> requiredEscapes, string startDelimiter = "", string endDelimiter = "")
        {
            // Remove enclosing delimiters.
            TextSpan contents = token.Unpack(startDelimiter.Length, endDelimiter.Length);

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
                    chr = ParseHex(unicodeHex, HexParseMode.Uppercase);
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
        private static void ParseDate(TextSpan date, out IntValue year, out IntValue month, out IntValue day)
        {
            // Parse year.
            int endOfYear = date.FirstIndexOf('/');
            if (endOfYear == -1)
                throw new FormatException($"Date does not contain a month.");

            TextSpan yearText = date.Slice(0, endOfYear);
            if (yearText.Length == 0)
                throw new FormatException($"Empty year term {yearText.ToString()}.");
            if (GetNumericType(yearText) != NumericType.Int)
                throw new FormatException($"Non-integer year {yearText.ToString()}.");

            year = IntValue.Parse(yearText);
            if (year == 0)
                throw new FormatException("Year may not be 0");

            // Parse month.
            int endOfMonth = date.FirstIndexOf(endOfYear + 1, '/');
            if (endOfMonth == -1)
                throw new FormatException($"Date does not contain a day.");

            TextSpan monthText = date.Slice(endOfYear + 1, endOfMonth - (endOfYear + 1));
            if (monthText.Length == 0)
                throw new FormatException($"Empty month term {monthText.ToString()}.");
            if (monthText.StartsWith('-'))
                throw new FormatException($"Negative month {monthText.ToString()}.");
            if (GetNumericType(monthText) != NumericType.Int)
                throw new FormatException($"Non-integer month {monthText.ToString()}.");
            if (!IsWithinRange(monthText, 1, 12))
                throw new FormatException($"Month must be in range [1-12], but equals {monthText.ToString()}.");

            month = IntValue.Parse(monthText);

            // Parse day.
            TextSpan dayText = date.Slice(endOfMonth + 1);
            if (dayText.Length == 0)
                throw new FormatException($"Empty day term {dayText.ToString()}.");
            if (dayText.StartsWith('-'))
                throw new FormatException($"Negative day {dayText.ToString()}.");
            if (GetNumericType(dayText) != NumericType.Int)
                throw new FormatException($"Non-integer day {dayText.ToString()}.");
            if (!IsWithinRange(dayText, 1, 31))
                throw new FormatException($"Day must be in range [1-31], but equals {dayText.ToString()}.");

            day = IntValue.Parse(dayText);
        }

        /// <summary>
        /// Parse a time component.
        /// </summary>
        private static void ParseTime(TextSpan time, out IntValue hour, out IntValue minute, out FloatValue second)
        {
            // Parse hour.
            int endOfHour = time.FirstIndexOf(':');
            if (endOfHour == -1)
                throw new FormatException($"Time does not contain an hour.");

            TextSpan hourText = time.Slice(0, endOfHour);
            if (hourText.Length == 0)
                throw new FormatException($"Empty hour term {hourText.ToString()}.");
            if (hourText.StartsWith('-'))
                throw new FormatException($"Negative hour {hourText.ToString()}.");
            if (GetNumericType(hourText) != NumericType.Int)
                throw new FormatException($"Non-integer hour {hourText.ToString()}.");
            if (!IsWithinRange(hourText, 0, 24))
                throw new FormatException($"Hour must be in range [0-24], but equals {hourText.ToString()}.");

            hour = IntValue.Parse(hourText);

            // Parse minute.
            int endOfMinute = time.FirstIndexOf(endOfHour + 1, ':');
            if (endOfMinute == -1)
                throw new FormatException($"Time does not contain a minute.");

            TextSpan minuteText = time.Slice(endOfHour + 1, endOfMinute - (endOfHour + 1));
            if (minuteText.Length == 0)
                throw new FormatException($"Empty minute term {minuteText.ToString()}.");
            if (minuteText.StartsWith('-'))
                throw new FormatException($"Negative minute {minuteText.ToString()}.");
            if (GetNumericType(minuteText) != NumericType.Int)
                throw new FormatException($"Minute must be an integer {minuteText.ToString()}");
            if (!IsWithinRange(minuteText, 0, 59))
                throw new FormatException($"Minute must be in range [0-59], but equals  {minuteText.ToString()}.");
            if (IsWithinRange(hourText, 24, 24) && !IsWithinRange(minuteText, 0, 0))
                throw new FormatException($"Minute must be 0 if hour is 24, but equals {minuteText.ToString()}.");

            minute = IntValue.Parse(minuteText);

            // Parse second.
            TextSpan secondText = time.Slice(endOfMinute + 1);
            if (secondText.Length == 0)
                throw new FormatException($"Empty second term.");
            if (secondText.StartsWith('-'))
                throw new FormatException($"Negative second {secondText.ToString()}.");
            if (GetNumericType(secondText, NumericParseMode.AllowLonePoint) == NumericType.NaN)
                throw new FormatException($"Non-numeric second {secondText.ToString()}.");
            secondText = ProcessReal(secondText).AsSpan();

            int pointIndex = secondText.FirstIndexOf('.');
            TextSpan secondInt = pointIndex == -1 ? secondText : secondText.Slice(0, pointIndex);
            if (!IsWithinRange(secondInt, 0, 60))
                throw new FormatException($"Second must be in the range [0-60], but equals {secondText.ToString()}.");

            TextSpan secondFrac = pointIndex == -1 ? "0" : secondText.Slice(pointIndex + 1);
            if (IsWithinRange(hourText, 24, 24) && (!IsWithinRange(secondInt, 0, 0) || !IsWithinRange(secondFrac, 0, 0)))
                throw new FormatException($"Second must be 0 if hour is 24, but equals {secondText.ToString()}.");

            second = FloatValue.Parse(secondText);
        }
    }
}