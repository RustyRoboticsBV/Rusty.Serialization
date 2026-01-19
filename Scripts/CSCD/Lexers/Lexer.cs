using System;
using Rusty.Serialization.Core.Lexer;

namespace Rusty.Serialization.CSCD.Lexer
{
    /// <summary>
    /// A CSCD lexer. It breaks a string of CSCD down into interpunction and "word" tokens.
    /// Word tokens can be any primitive value literal or metadata literal.
    /// The lexer only does limited syntactic analyis, throwing only when delimited tokens are not closed properly.
    /// No semantic analysis is performed.
    /// </summary>
    public class Lexer : Core.Lexer.Lexer
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

            // Start parsing next token.
            char c = Current(text);

            // Interpunction.
            if (c == ',' || c == ':' || c == '[' || c == ']' || c == '{' || c == '}' || c == '<' || c == '>')
                token = MakeTokenAndAdvance(text, 1);

            // Delimited word tokens.
            else if (c == '(')
                token = MakeTokenAndAdvance(text, ReadDelimitedToken(text, ')'));
            else if (c == '`')
                token = MakeTokenAndAdvance(text, ReadDelimitedToken(text, '`'));
            else if (c == '\'')
                token = MakeTokenAndAdvance(text, ReadDelimitedToken(text, '\''));
            else if (c == '"')
                token = MakeTokenAndAdvance(text, ReadDelimitedToken(text, '"'));
            else if (c == '&')
                token = MakeTokenAndAdvance(text, ReadDelimitedToken(text, ';'));

            // Bare word tokens.
            else
                token = MakeTokenAndAdvance(text, ReadBareToken(text));

            return true;
        }

        /* Private methods. */
        /// <summary>
        /// Create a token and advance the cursor. Also skips trailing whitespace.
        /// </summary>
        private Token MakeTokenAndAdvance(TextSpan text, int length)
        {
            if (length < 0)
                throw new FormatException($"Zero-length token at {Cursor}: {new string(text.Slice(Cursor))}.");

            Token token = MakeToken(length);
            Advance(token.Length);
            return token;
        }

        /// <summary>
        /// Check if the a substring starts with a whitespace at some index. Comments are considered to be whitespace.
        /// </summary>
        private static bool StartsWithWhitespace(TextSpan text, int index)
        {
            if (index >= text.Length)
                return false;

            char c = text[index];
            return c == ' ' || c == '\t' || c == '\n' || c == '\r' || text.StartsWith(index, "/*");
        }

        /// <summary>
        /// Skip whitespace. Comments are considered to be whitespace.
        /// </summary>
        private void SkipWhitespace(TextSpan text)
        {
            if (IsAtEnd(text))
                return;

            while (StartsWithWhitespace(text, Cursor))
            {
                // Comment.
                if (text.StartsWith(Cursor, "/*"))
                {
                    int end = text.FirstIndexOf(Cursor + 2, "*/");
                    if (end == -1)
                        throw new FormatException($"Unclosed comment at {Cursor}: {new string(text.Slice(Cursor))}.");

                    Advance(end - Cursor + 2);
                }

                // Whitespace.
                else
                    Advance();

                // Stop on end.
                if (IsAtEnd(text))
                    return;
            }
        }

        /// <summary>
        /// Read a delimited token and return the length. The cursor is NOT advanced.
        /// </summary>
        private int ReadDelimitedToken(TextSpan text, char delimiter)
        {
            for (int i = Cursor + 1; i < text.Length; i++)
            {
                char c = text[i];

                // Closing delimiter.
                if (c == delimiter)
                    return i - Cursor + 1;

                // Escaped character.
                else if (i + 1 < text.Length && c == '\\')
                    i++;
            }

            throw new FormatException($"Unclosed delimited token at {Cursor}: {new string(text.Slice(Cursor))}.");
        }

        /// <summary>
        /// Read a bare token and return the length. The cursor is NOT advanced.
        /// </summary>
        private int ReadBareToken(TextSpan text)
        {
            for (int i = Cursor; i < text.Length; i++)
            {
                // Check for trailing whitespace.
                if (StartsWithWhitespace(text, i))
                    return i - Cursor;

                // Check for interpunction.
                switch (text[i])
                {
                    case ',':
                    case ':':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                    case '<':
                    case '>':
                        return i - Cursor;
                }
            }
            return text.Length - Cursor;
        }
    }
}