using System;

namespace Rusty.Serialization.Core.Lexing
{
    /// <summary>
    /// A lexeme - a range over a text that represents a unit within a serialized string.
    /// </summary>
    public readonly struct Lexeme
    {
        /* Fields. */
        /// <summary>
        /// The start character index of the lexeme in the source text.
        /// </summary>
        public readonly int Start;
        /// <summary>
        /// The character length of the lexeme in the source text.
        /// </summary>
        public readonly int Length;

        /// <summary>
        /// The end character index of the lexeme in the source text.
        /// </summary>
        public int End => Start + Length;

        /* Constructors. */
        public Lexeme(int start, int length)
        {
            Start = start;
            Length = length;
        }

        /* Public methods. */
        /// <summary>
        /// Extract the lexeme's text from a source text.
        /// </summary>
        public TextSpan ExtractFrom(TextSpan source) => source.Slice(Start, Length);
    }
}