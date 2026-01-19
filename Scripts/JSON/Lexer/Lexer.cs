using System;
using Rusty.Serialization.Core.Lexer;

namespace Rusty.Serialization.Json.Lexer
{
    public ref struct JsonLexer
    {
        /* Fields. */
        private TextCursor cursor;

        /* Public properties. */
        /// <summary>
        /// Get if the source text has been fully tokenized.
        /// </summary>
        public bool IsAtEnd => cursor.IsAtEnd;

        /* Constructors. */
        public JsonLexer(ReadOnlySpan<char> text)
        {
            cursor = new TextCursor(text);
        }

        /// <summary>
        /// Lexes the next token. Returns false if the source text has been fully tokenized.
        /// </summary>
        public bool NextToken(out Token<TokenType> token)
        {
            // Ignore whitespace.
            SkipWhitespace();

            // If done, return false.
            if (cursor.IsAtEnd)
            {
                token = default;
                return false;
            }

            // Get character at cursor position.
            char c = cursor.Current;

            // Punctuation.
            switch (c)
            {
                case '{':
                    token = MakeToken(TokenType.ObjectStart, 1);
                    cursor.Advance();
                    return true;
                case '}':
                    token = MakeToken(TokenType.ObjectEnd, 1);
                    cursor.Advance();
                    return true;
                case '[':
                    token = MakeToken(TokenType.ArrayStart, 1);
                    cursor.Advance();
                    return true;
                case ']':
                    token = MakeToken(TokenType.ArrayEnd, 1);
                    cursor.Advance();
                    return true;
                case ',':
                    token = MakeToken(TokenType.Comma, 1);
                    cursor.Advance();
                    return true;
                case ':':
                    token = MakeToken(TokenType.Colon, 1);
                    cursor.Advance();
                    return true;
            }

            // Null.
            if (c == 'n' && cursor.StartsWith("null"))
            {
                token = MakeToken(TokenType.Null, 4);
                cursor.Advance(4);
                return true;
            }

            // Boolean.
            if (c == 't' && cursor.StartsWith("true"))
            {
                token = MakeToken(TokenType.True, 4);
                cursor.Advance(4);
                return true;
            }
            if (c == 'f' && cursor.StartsWith("false"))
            {
                token = MakeToken(TokenType.False, 5);
                cursor.Advance(5);
                return true;
            }

            // Number.
            if (IsDigit(c) || c == '-')
            {
                int start = cursor.Position;
                ReadNumber();
                int length = cursor.Position - start;
                token = new Token<TokenType>(TokenType.Number, start, length);
                return true;
            }

            // String.
            if (c == '"')
            {
                int start = cursor.Position;
                if (!ReadString())
                    throw new FormatException("Unterminated string.");
                int length = cursor.Position - start;
                token = new Token<TokenType>(TokenType.String, start, length);
                return true;
            }

            throw new FormatException($"Unexpected character '{c}' at position {cursor.Position}.");
        }

        /* Private methods. */
        private Token<TokenType> MakeToken(TokenType type, int length)
        {
            return new Token<TokenType>(type, cursor.Position, length);
        }

        private void SkipWhitespace()
        {
            while (!cursor.IsAtEnd)
            {
                char c = cursor.Current;
                if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
                    cursor.Advance();
                else
                    break;
            }
        }

        private void ReadNumber()
        {
            // Integer part.
            if (cursor.Current == '-')
                cursor.Advance();
            
            if (!IsDigit(cursor.Current))
                throw new FormatException("Invalid number: expected digit after optional minus.");

            if (cursor.Current == '0' && cursor.CanPeek && IsDigit(cursor.Next))
                throw new FormatException("Invalid number: leading zeros are not allowed.");

            while (!cursor.IsAtEnd && IsDigit(cursor.Current))
            {
                cursor.Advance();
            }

            // Fraction.
            if (!cursor.IsAtEnd && cursor.Current == '.')
            {
                cursor.Advance();
                if (cursor.IsAtEnd || !IsDigit(cursor.Current))
                    throw new FormatException("Invalid number: fraction must have at least one digit.");

                while (!cursor.IsAtEnd && IsDigit(cursor.Current))
                {
                    cursor.Advance();
                }
            }

            // Exponent.
            if (!cursor.IsAtEnd && (cursor.Current == 'e' || cursor.Current == 'E'))
            {
                cursor.Advance();
                if (!cursor.IsAtEnd && (cursor.Current == '+' || cursor.Current == '-'))
                    cursor.Advance();

                if (cursor.IsAtEnd || !IsDigit(cursor.Current))
                    throw new FormatException("Invalid number: exponent must have at least one digit.");

                while (!cursor.IsAtEnd && IsDigit(cursor.Current))
                {
                    cursor.Advance();
                }
            }
        }

        private bool ReadString()
        {
            cursor.Advance();
            while (!cursor.IsAtEnd)
            {
                char c = cursor.Current;

                // Closing quote.
                if (c == '"')
                {
                    cursor.Advance();
                    return true;
                }

                // Escape sequence.
                else if (c == '\\')
                {
                    cursor.Advance();
                    if (cursor.IsAtEnd)
                        return false;
                    cursor.Advance();
                }
                else
                {
                    cursor.Advance();
                }
            }
            return false;
        }

        private static bool IsDigit(char c) => c >= '0' && c <= '9';
    }
}
