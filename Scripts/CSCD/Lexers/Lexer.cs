using System;
using Rusty.Serialization.Core.Lexer;

namespace Rusty.Serialization.CSCD.Lexer
{
    /// <summary>
    /// A CSCD lexer.
    /// </summary>
    public class Lexer : Lexer<TokenType>
    {
        /* Public methods. */
        public override bool GetNextToken(ref TextCursor cursor, out Token<TokenType> token)
        {
            int length = 0;
            token = default;

            // Do nothing if at end.
            if (cursor.IsAtEnd)
                return false;

            char c = cursor.Current;

            // Skip leading whitespace.
            SkipWhitespace(ref cursor);

            // Do nothing if at end.
            if (cursor.IsAtEnd)
                return false;

            // Type.
            else if (cursor.StartsWith('('))
            {
                length = ReadDelimitedLiteral(cursor, ')');
                token = MakeToken(TokenType.Literal, cursor, length);
                cursor.Advance(token.Length);
                SkipWhitespace(ref cursor);
                return true;
            }

            // ID.
            else if (cursor.StartsWith('`'))
            {
                length = ReadDelimitedLiteral(cursor, '`');
                token = MakeToken(TokenType.Literal, cursor, length);
                cursor.Advance(token.Length);
                SkipWhitespace(ref cursor);
                return true;
            }

            // Char.
            else if (cursor.StartsWith('\''))
            {
                length = ReadDelimitedLiteral(cursor, '\'');
                token = MakeToken(TokenType.Literal, cursor, length);
                cursor.Advance(token.Length);
                SkipWhitespace(ref cursor);
                return true;
            }

            // String.
            else if (cursor.StartsWith('"'))
            {
                length = ReadDelimitedLiteral(cursor, '"');
                token = MakeToken(TokenType.Literal, cursor, length);
                cursor.Advance(token.Length);
                SkipWhitespace(ref cursor);
                return true;
            }

            // Ref.
            else if (cursor.StartsWith('&'))
            {
                length = ReadDelimitedLiteral(cursor, ';');
                token = MakeToken(TokenType.Literal, cursor, length);
                cursor.Advance(token.Length);
                SkipWhitespace(ref cursor);
                return true;
            }

            // Interpunction.
            switch (c)
            {
                case ',':
                    token = MakeToken(TokenType.Comma, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
                case ':':
                    token = MakeToken(TokenType.Colon, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
                case '[':
                    token = MakeToken(TokenType.ListStart, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
                case ']':
                    token = MakeToken(TokenType.ListEnd, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
                case '{':
                    token = MakeToken(TokenType.DictStart, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
                case '}':
                    token = MakeToken(TokenType.DictEnd, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
                case '<':
                    token = MakeToken(TokenType.ObjectStart, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
                case '>':
                    token = MakeToken(TokenType.ObjectEnd, cursor, 1);
                    cursor.Advance(token.Length);
                    SkipWhitespace(ref cursor);
                    return true;
            }

            // Other literal types.
            length = ReadBareLiteral(cursor);
            token = MakeToken(TokenType.Literal, cursor, length);
            cursor.Advance(token.Length);
            SkipWhitespace(ref cursor);
            return true;
        }

        /* Private methods. */
        /// <summary>
        /// Check if the a substring starts with a whitespace at some index. Comments are considered to be whitespace.
        /// </summary>
        private static bool StartsWithWhitespace(ref TextCursor cursor, int index)
        {
            char c = cursor[index];
            return c == ' ' || c == '\t' || c == '\n' || c == '\r' || cursor.StartsWith(index, "/*");
        }

        /// <summary>
        /// Skip whitespace. Comments are considered to be whitespace.
        /// </summary>
        private static void SkipWhitespace(ref TextCursor cursor)
        {
            if (cursor.IsAtEnd)
                return;

            char c = cursor.Current;

            while (StartsWithWhitespace(ref cursor, cursor.Position))
            {
                // Comment.
                if (cursor.StartsWith("/*"))
                {
                    int end = cursor.FirstIndexOf(cursor.Position + 2, "*/");
                    if (end == -1)
                        throw new FormatException($"Unclosed comment at {cursor.Position}: {new string(cursor.Slice())}.");

                    cursor.Advance(end - cursor.Position + 2);
                }

                // Whitespace.
                else
                    cursor.Advance();

                // Stop on end.
                if (cursor.IsAtEnd)
                    return;

                // Get next character.
                c = cursor.Current;
            }
        }

        /// <summary>
        /// Read a delimited literal.
        /// </summary>
        private static int ReadDelimitedLiteral(TextCursor cursor, char delimiter)
        {
            for (int i = cursor.Position; i < cursor.Length; i++)
            {
                char c = cursor[i];

                // Closing delimiter.
                if (c == delimiter)
                    return i - cursor.Position + 1;

                // Escaped character.
                else if (i + 1 < cursor.Length && c == '\\')
                    i++;
            }

            throw new FormatException($"Unclosed delimited literal at {cursor.Position}: {new string(cursor.Slice())}.");
        }

        /// <summary>
        /// Read a bare literal.
        /// </summary>
        private static int ReadBareLiteral(TextCursor cursor)
        {
            for (int i = cursor.Position; i < cursor.Length; i++)
            {
                // Check for trailing whitespace.
                if (StartsWithWhitespace(ref cursor, i))
                    return i - cursor.Position;

                // Check for interpunction.
                switch (cursor[i])
                {
                    case '\t':
                    case '\n':
                    case '\r':
                    case ',':
                    case ':':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                    case '<':
                    case '>':
                        return i - cursor.Position;
                }
            }
            return cursor.Length - cursor.Position;
        }
    }
}