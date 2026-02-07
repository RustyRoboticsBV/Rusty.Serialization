using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Codecs
{
    /// <summary>
    /// A base class for a token parser.
    /// </summary>
    public abstract class Parser<LexerT>
        where LexerT : Lexer
    {
        /* Public methods. */
        /// <summary>
        /// Parse a text as a node tree.
        /// </summary>
        public abstract NodeTree Parse(TextSpan span, LexerT lexer);

        /* Protected types. */
        protected enum NumericType { NaN, Int, Real };

        protected enum HexParseMode
        {
            /// <summary>
            /// Allow uppercase, lowercase letters and digits.
            /// </summary>
            UppercaseAndLowercase,
            /// <summary>
            /// Allow only uppercase letters and digits.
            /// </summary>
            Uppercase,
            /// <summary>
            /// Allow only lowercase letters and digits.
            /// </summary>
            Lowercase
        };
        protected enum NumericParseMode
        {
            /// <summary>
            /// Allow only full decimal number notations (i.e. 1.5).
            /// </summary>
            AllowFullNotationOnly,
            /// <summary>
            /// Allow full decimal number notations (i.e. 1.5) and leading decimal point notations (i.e. .5).
            /// </summary>
            AllowLeadingPointOnly,
            /// <summary>
            /// Allow full decimal number notations (i.e. 1.5) and trailing decimal point notations (i.e. 1.).
            /// </summary>
            AllowTrailingPointOnly,
            /// <summary>
            /// Allow full decimal number notations (i.e. 1.5), leading decimal point notations (i.e. .5) and trailing decimal
            /// point notations (i.e. 1.).
            /// </summary>
            AllowLeadingAndTrailingPoint,
            /// <summary>
            /// Allow full decimal number notations (i.e. 1.5), leading decimal point notations (i.e. .5), trailing decimal
            /// point notations (i.e. 1.) and lone point notations as substitutes for zero (i.e. . and -.).
            /// </summary>
            AllowLonePoint
        };

        /* Protected methods. */
        #region TOKEN_VALIDATION
        /// <summary>
        /// Throw a format exception related to some token.
        /// </summary>
        protected static FormatException TokenError(Token token, string errorMessage)
        {
            if (token.IsEOF)
                throw new FormatException($"At EOF ({token.ToString()}): {errorMessage}");
            throw new FormatException($"At {token.Lexeme.Start} ({token.ToString()}): {errorMessage}");
        }

        /// <summary>
        /// Try to get a token and throw a format exception on failure.
        /// </summary>
        protected static Token ExpectToken(TextSpan text, LexerT lexer, string errorMessage)
        {
            if (!lexer.GetNextToken(text, out Token token))
                TokenError(Token.EOF, errorMessage);
            return token;
        }

        /// <summary>
        /// Try to get a token, check if it equals a symbol and throw a format exception on failure.
        /// </summary>
        protected static Token ExpectSymbol(TextSpan text, LexerT lexer, char symbol, string errorMessage)
        {
            Token token = ExpectToken(text, lexer, errorMessage);
            if (!token.Text.Equals(symbol))
                TokenError(token, errorMessage);
            return token;
        }

        /// <summary>
        /// Throw a format exception if a token does not equal some character.
        /// </summary>
        protected static void MustEqual(Token token, char chr, string errorMessage)
        {
            if (!token.Text.Equals(chr))
                TokenError(token, errorMessage);
        }

        /// <summary>
        /// Throw a format exception if a token equals some character.
        /// </summary>
        protected static void DisallowEqual(Token token, char chr, string errorMessage)
        {
            if (token.Text.Equals(chr))
                TokenError(token, errorMessage);
        }
        #endregion

        #region PARSING_METHODS
        /// <summary>
        /// Get the numeric type of a string ("Int" or "Real"). Returns "NaN" if not numeric.
        /// </summary>
        protected static NumericType GetNumericType(TextSpan span, NumericParseMode mode = NumericParseMode.AllowTrailingPointOnly)
        {
            // First classify according to the most permissive mode (AllowLonePoint).
            int i = 0;
            bool fractional = false;

            if (span.Length == 0)
                return NumericType.NaN;

            // Leading minus sign.
            if (span[0] == '-')
            {
                if (span.Length == 1)
                    return NumericType.NaN;
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
                        return NumericType.NaN;
                }

                // Not numeric.
                else if (span[i] < '0' || span[i] > '9')
                    return NumericType.NaN;
            }

            NumericType type = fractional ? NumericType.Real : NumericType.Int;
            if (type == NumericType.Int)
                return type;

            // If real, restrict based on mode.
            switch (mode)
            {
                case NumericParseMode.AllowFullNotationOnly:
                    if (span.StartsWith('.') || span.StartsWith("-.") || span.EndsWith('.'))
                        return NumericType.NaN;
                    return type;
                case NumericParseMode.AllowLeadingPointOnly:
                    if (span.EndsWith('.'))
                        return NumericType.NaN;
                    return type;
                case NumericParseMode.AllowTrailingPointOnly:
                    if (span.StartsWith('.') || span.StartsWith("-."))
                        return NumericType.NaN;
                    return type;
                case NumericParseMode.AllowLeadingAndTrailingPoint:
                    if (span.Equals('.') || span.Equals("-."))
                        return NumericType.NaN;
                    return type;
                default:
                    return type;
            }
        }

        /// <summary>
        /// Parse a hex digit.
        /// </summary>
        protected static byte ParseHexDigit(char c, HexParseMode mode = HexParseMode.UppercaseAndLowercase)
        {
            if (c >= '0' && c <= '9')
                return (byte)(c - '0');
            if (c >= 'A' && c <= 'F' && mode != HexParseMode.Lowercase)
                return (byte)(c - 'A' + 10);
            if (c >= 'a' && c <= 'f' && mode != HexParseMode.Uppercase)
                return (byte)(c - 'a' + 10);
            throw new ArgumentOutOfRangeException(nameof(c), $"Invalid hex digit '{c}'");
        }

        /// <summary>
        /// Parse a hex number.
        /// </summary>
        protected static int ParseHex(TextSpan hex, HexParseMode mode = HexParseMode.UppercaseAndLowercase)
        {
            if (hex.Length == 0)
                throw new FormatException("Cannot parse empty hex strings.");

            int result = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                char c = hex[i];
                int value = ParseHexDigit(c, mode);
                result = (result << 4) | value;
            }

            return result;
        }

        /// <summary>
        /// Checks if the textual representation of an unsigned integer  is within the inclusive range [min, max].
        /// </summary>
        public static bool IsWithinRange(TextSpan span, uint min, uint max)
        {
            if (span.Length == 0)
                return false;

            // Skip leading zeros
            int start = 0;
            while (start < span.Length && span[start] == '0') start++;
            ReadOnlySpan<char> trimmed = span.Slice(start);

            // Treat all zeros as "0"
            if (trimmed.Length == 0) trimmed = "0";

            // Check that every char is a digit
            for (int i = 0; i < trimmed.Length; i++)
            {
                char c = trimmed[i];
                if (c < '0' || c > '9')
                    return false;
            }

            // Count digits
            int len = trimmed.Length;

            // Quick bounds check based on number of digits
            if (len > 10)
                return false;
            if (len < 1)
                return false;

            // Compare numerically without string conversion.
            uint value = 0;
            foreach (char c in trimmed)
            {
                uint digit = (uint)(c - '0');

                // Check for overflow before multiplying.
                if (value > (uint.MaxValue - digit) / 10)
                    return false;

                value = value * 10 + digit;
            }

            return value >= min && value <= max;
        }

        /// <summary>
        /// Parse a color channel in hex format.
        /// </summary>
        protected static byte ParseColorChannel(TextSpan span)
        {
            if (span.Length < 1 || span.Length > 2)
                throw new ArgumentOutOfRangeException(nameof(span));

            if (span.Length == 1)
            {
                byte shorthand = ParseHexDigit(span[0], HexParseMode.Uppercase);
                return (byte)((shorthand << 4) | shorthand);
            }
            else
                return (byte)ParseHex(span, HexParseMode.Uppercase);
        }
        #endregion
    }
}