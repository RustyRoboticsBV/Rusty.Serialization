using System;
using Rusty.Serialization.Core.Codecs;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON lexer. It breaks a string of JSON down into interpunction tokens and other tokens.
    /// The lexer only does limited syntactic analyis, throwing only when delimited tokens are not closed properly.
    /// No semantic analysis is performed.
    /// </summary>
    public class JsonLexer : Lexer
    {
        /* Public methods. */
        public override bool GetNextToken(TextSpan text, out Token token)
        {
            token = default;

            // Do nothing if at end.
            if (IsAtEnd(text))
                return false;

            // Skip leading whitespace.
            SkipWhitespace(text);

            // Do nothing if at end.
            if (IsAtEnd(text))
                return false;

            char c = Current(text);

            // Punctuation.
            if (c == ',' || c == ':' || c == '[' || c == ']' || c == '{' || c == '}')
                token = MakeTokenAndAdvance(text, 1);

            // String literal.
            else if (c == '"')
                token = MakeTokenAndAdvance(text, ReadStringLexeme(text));

            // Bare value: number, boolean, null.
            else
                token = MakeTokenAndAdvance(text, ReadBareLexeme(text));

            return true;
        }

        /* Private methods. */
        /// <summary>
        /// Create a token and advance the cursor.
        /// </summary>
        private Token MakeTokenAndAdvance(TextSpan text, int length)
        {
            if (length < 0)
                throw new FormatException($"Zero-length token at {Cursor}: {new string(text.Slice(Cursor))}.");

            Lexeme lexeme = MakeLexeme(length);
            Advance(lexeme.Length);
            return new Token(text, lexeme);
        }

        /// <summary>
        /// Skip whitespaces at the cursor.
        /// </summary>
        private void SkipWhitespace(TextSpan text)
        {
            while (!IsAtEnd(text) && IsWhitespace(Current(text)))
                Advance();
        }

        /// <summary>
        /// Check if a character is a whitespace.
        /// </summary>
        private static bool IsWhitespace(char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';

        /// <summary>
        /// Read a string lexeme and return the length. The cursor is NOT advanced.
        /// </summary>
        private int ReadStringLexeme(TextSpan text)
        {
            int start = Cursor + 1;

            for (int i = start; i < text.Length; i++)
            {
                char c = text[i];

                // Closing delimiter.
                if (c == '"')
                    return i - Cursor + 1;

                // Escaped character.
                if (c == '\\' && i + 1 < text.Length)
                    i++;
            }

            throw new FormatException($"Unclosed string literal at {Cursor}: {new string(text.Slice(Cursor))}.");
        }

        /// <summary>
        /// Read a bare lexeme.
        /// </summary>
        private int ReadBareLexeme(TextSpan text)
        {
            for (int i = Cursor; i < text.Length; i++)
            {
                char c = text[i];

                if (IsWhitespace(c) || c == ',' || c == ':' || c == '[' || c == ']' || c == '{' || c == '}')
                    return i - Cursor;
            }

            return text.Length - Cursor;
        }
    }
}