using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Lexer
{
    /// <summary>
    /// A token stream.
    /// </summary>
    public class TokenStream<TokenType>
        where TokenType : unmanaged
    {
        /* Fields. */
        private List<Token<TokenType>> tokens = new();

        /* Public properties. */
        public int Count => tokens.Count;

        /* Indexers. */
        public Token<TokenType> this[int index] => tokens[index];

        /* Constructors. */
        public TokenStream(string text, Lexer<TokenType> lexer) : this(text.AsSpan(), lexer) { }

        public TokenStream(ReadOnlySpan<char> text, Lexer<TokenType> lexer)
        {
            TextCursor cursor = Lexer<TokenType>.GetCursor(text);

            while (lexer.GetNextToken(ref cursor, out Token<TokenType> token))
            {
                tokens.Add(token);
            }
        }

        /* Public methods. */
        public string ToString(ReadOnlySpan<char> text)
        {
            string str = "";
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i > 0)
                    str += ", ";
                str += '[' + tokens[i].ToString(text) + ']';
            }
            return str;
        }

        public string ToString(string text) => ToString(text.AsSpan());
    }
}