using System;
using Rusty.Serialization.Core.Codecs;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD lexer. It breaks a string of CSCD down into interpunction tokens and other tokens.
    /// The lexer only does limited syntactic analyis, throwing only when delimited tokens are not closed properly.
    /// No semantic analysis is performed.
    /// </summary>
    public class CscdLexer : Lexer
    {
        /* Private properties. */
        private int State { get; set; }

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

            // Comments.
            if (c == ';' && Cursor + 1 < text.Length && Next(text) == ';')
            {
                token = MakeTokenAndAdvance(text, ReadComment(text));

                // Skip comments inside of the main block.
                if (State == 1)
                    GetNextToken(text, out token);
            }

            // Interpunction.
            else if (c == ',' || c == ':' || c == '[' || c == ']' || c == '{' || c == '}' || c == '<' || c == '>')
                token = MakeTokenAndAdvance(text, 1);

            // Shorthand quote character.
            else if (c == '\'' && Cursor + 2 < text.Length && Next(text) == '\'' && NextNext(text) == '\'')
                token = MakeTokenAndAdvance(text, 3);

            // Delimited word tokens.
            else if (c == '~')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '~'));
            else if (c == '`')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '`'));
            else if (c == '(')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, ')'));
            else if (c == '^')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '^'));
            else if (c == '|')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '|'));
            else if (c == '\'')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '\''));
            else if (c == '"')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '"'));
            else if (c == '@')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '@'));
            else if (c == '*')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '*'));
            else if (c == '&')
                token = MakeTokenAndAdvance(text, ReadDelimitedLexeme(text, '&'));

            // Bare word tokens.
            else
                token = MakeTokenAndAdvance(text, ReadBareLexeme(text));

            if (token.Text.Equals("~/CSCD~"))
                State = 2;
            else if (State == 0)
                State = 1;
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
        /// Check if the a substring starts with a whitespace or comment at some index.
        /// </summary>
        private static bool StartsWithWhitespace(TextSpan text, int index)
        {
            if (index >= text.Length)
                return false;

            char c = text[index];
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        /// <summary>
        /// Skip whitespace.
        /// </summary>
        private void SkipWhitespace(TextSpan text)
        {
            if (IsAtEnd(text))
                return;

            while (StartsWithWhitespace(text, Cursor))
            {
                Advance();

                // Stop on end.
                if (IsAtEnd(text))
                    return;
            }
        }

        /// <summary>
        /// Read a delimited lexeme and return the length. The cursor is NOT advanced.
        /// </summary>
        private int ReadComment(TextSpan text)
        {
            for (int i = Cursor + 2; i < text.Length - 1; i++)
            {
                // Closing delimiter.
                if (text[i] == ';' && text[i + 1] == ';')
                    return i - Cursor + 2;
            }

            throw new FormatException($"Unclosed comment at {Cursor}: {new string(text.Slice(Cursor))}.");
        }

        /// <summary>
        /// Read a delimited lexeme and return the length. The cursor is NOT advanced.
        /// </summary>
        private int ReadDelimitedLexeme(TextSpan text, char delimiter)
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
        /// Read a bare lexeme and return the length. The cursor is NOT advanced.
        /// </summary>
        private int ReadBareLexeme(TextSpan text)
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