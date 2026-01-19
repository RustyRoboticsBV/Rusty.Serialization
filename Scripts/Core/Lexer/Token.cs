using System;

namespace Rusty.Serialization.Core.Lexer
{
    /// <summary>
    /// A lexer token.
    /// </summary>
    public readonly struct Token
    {
        /* Fields. */
        /// <summary>
        /// The start character index of the token in the source data.
        /// </summary>
        public readonly int Start;
        /// <summary>
        /// The character length of the token in the source data.
        /// </summary>
        public readonly int Length;

        /* Constructors. */
        public Token(int start, int length)
        {
            Start = start;
            Length = length;
        }

        /* Public methods. */
        /// <summary>
        /// Extract the token from a source text.
        /// </summary>
        public ReadOnlySpan<char> ExtractFrom(ReadOnlySpan<char> source) => source.Slice(Start, Length);
    }
}