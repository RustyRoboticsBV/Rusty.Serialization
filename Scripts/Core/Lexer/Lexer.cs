using System;

namespace Rusty.Serialization.Core.Lexer
{
    /// <summary>
    /// A base class for lexers.
    /// </summary>
    public abstract class Lexer<TokenType>
        where TokenType : unmanaged
    {
        /* Public methods. */
        /// <summary>
        /// Create a lexer cursor.
        /// </summary>
        public static TextCursor GetCursor(ReadOnlySpan<char> text) => new TextCursor(text);

        /// <summary>
        /// Lexes the next token. Throws if there is no token left.
        /// </summary>
        public abstract bool GetNextToken(ref TextCursor cursor, out Token<TokenType> token);

        /* Protected methods. */
        protected static bool IsAsciiWhitespace(char c) => c <= 0x7F && char.IsWhiteSpace(c);
        protected static bool IsAsciiLetter(char c) => c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z';
        protected static bool IsAsciiDigit(char c) => c >= '0' && c <= '9';

        protected Token<TokenType> MakeToken(TokenType type, TextCursor cursor, int length)
            => new Token<TokenType>(type, cursor.Position, length);
    }
}