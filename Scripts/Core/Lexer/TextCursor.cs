using System;

namespace Rusty.Serialization.Core.Lexer
{
    /// <summary>
    /// A lexer text cursor. It's essentially a wrapper around a char ReadOnlySpan with some useful methods.
    /// </summary>
    public ref struct TextCursor
    {
        /* Fields. */
        private readonly ReadOnlySpan<char> text;

        /* Public properties. */
        /// <summary>
        /// The cursor's current position in the text.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Whether or not the cursor has finished lexing all characters in the text.
        /// </summary>
        public bool IsAtEnd => Position >= text.Length;
        /// <summary>
        /// Whether or not at least one more character is left in the text after the current cursor position.
        /// </summary>
        public bool CanPeek => Position + 1 < text.Length;
        /// <summary>
        /// Get the character from the text at the cursor position.
        /// </summary>
        public char Current => text[Position];
        /// <summary>
        /// Get the next character from the text after the cursor position.
        /// </summary>
        public char Next => text[Position + 1];
        /// <summary>
        /// Get the length of the text.
        /// </summary>
        public int Length => text.Length;

        /* Constructors. */
        public TextCursor(ReadOnlySpan<char> text)
        {
            this.text = text;
            Position = 0;
        }

        /* Public methods. */
        /// <summary>
        /// Advance the cursor position with one character.
        /// </summary>
        public void Advance() => Advance(1);
        /// <summary>
        /// Advance the cursor position with some amount of characters.
        /// </summary>
        public void Advance(int count)
        {
            if (count < 0)
                throw new ArgumentException("Cannot advance with a negative number of characters.");
            Position += count;
        }

        /// <summary>
        /// Advance the cursor position with some amount of characters. Does not advance past the beginning or end of the string.
        /// </summary>
        public void AdvanceClamped() => AdvanceClamped(1);
        /// <summary>
        /// Advance the cursor position with some amount of characters. Does not advance past the beginning or end of the string.
        /// </summary>
        public void AdvanceClamped(int count)
        {
            Advance(count);
            if (Position < 0)
                Position = 0;
            if (Position > Length)
                Position = Length;
        }

        /// <summary>
        /// Get a substring of the text, starting at the current cursor position and continuing to the end of the text.
        /// </summary>
        public ReadOnlySpan<char> Slice() => Slice(Position, Length - Position);
        /// <summary>
        /// Get a substring of the text, starting at the current cursor position using some substring length.
        /// </summary>
        public ReadOnlySpan<char> Slice(int length) => Slice(Position, length);
        /// <summary>
        /// Get a substring of the text, starting at some character and using some substring length..
        /// </summary>
        public ReadOnlySpan<char> Slice(int start, int length) => text.Slice(start, length);

        /// <summary>
        /// Check if the text starting at the cursor begins with some substring.
        /// </summary>
        public bool StartsWith(ReadOnlySpan<char> substr)
        {
            if (Position + substr.Length > text.Length)
                return false;

            ReadOnlySpan<char> source = Slice(substr.Length);
            for (int i = 0; i < substr.Length; i++)
            {
                if (source[i] != substr[i])
                    return false;
            }

            return true;
        }
    }
}