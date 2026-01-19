using System;

namespace Rusty.Serialization.Core.Lexer
{
    /// <summary>
    /// A lexer token.
    /// </summary>
    public readonly struct Token<T>
        where T : unmanaged
    {
        /* Fields. */
        /// <summary>
        /// The type of token.
        /// </summary>
        public readonly T Type;
        /// <summary>
        /// The start character index of the token in the source data.
        /// </summary>
        public readonly int Start;
        /// <summary>
        /// The character length of the token in the source data.
        /// </summary>
        public readonly int Length;

        /* Constructors. */
        public Token(T type, int start, int length)
        {
            Type = type;
            Start = start;
            Length = length;
        }

        /* Public methods. */
        public ReadOnlySpan<char> GetText(ReadOnlySpan<char> source) => source.Slice(Start, Length);
    }
}