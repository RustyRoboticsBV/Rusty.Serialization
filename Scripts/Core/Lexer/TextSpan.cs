using System;

namespace Rusty.Serialization.Core.Lexing
{
    /// <summary>
    /// A sequence of text.
    /// </summary>
    public ref struct TextSpan
    {
        /* Fields. */
        private readonly ReadOnlySpan<char> span;

        /* Public properties. */
        /// <summary>
        /// Get the length of the text.
        /// </summary>
        public int Length => span.Length;

        /* Indexers. */
        public char this[int index] => span[index];

        /* Constructors. */
        public TextSpan(ReadOnlySpan<char> span)
        {
            this.span = span;
        }

        /* Casting operators. */
        public static implicit operator TextSpan(string str) => new TextSpan(str.AsSpan());
        public static implicit operator TextSpan(ReadOnlySpan<char> span) => new TextSpan(span);
        public static implicit operator ReadOnlySpan<char>(TextSpan textSpan) => textSpan.span;

        /* Public methods. */
        public override string ToString() => new string(span);

        /// <summary>
        /// Get a slice of the text, starting at some character and continuing to the end.
        /// </summary>
        public TextSpan Slice(int start) => new TextSpan(span.Slice(start));
        /// <summary>
        /// Get a slice of the text, starting at some character and using some substring length.
        /// </summary>
        public TextSpan Slice(int start, int length) => new TextSpan(span.Slice(start, length));

        /// <summary>
        /// Check if the text begins with some character.
        /// </summary>
        public bool StartsWith(char chr) => StartsWith(0, chr);
        /// <summary>
        /// Check if the text slice starting at some index begins with some substring.
        /// </summary>
        public bool StartsWith(int index, char chr) => Length > index && span[index] == chr;
        /// <summary>
        /// Check if the text begins with some substring.
        /// </summary>
        public bool StartsWith(ReadOnlySpan<char> substr) => StartsWith(span, substr);
        /// <summary>
        /// Check if the text slice starting at some index begins with some character.
        /// </summary>
        public bool StartsWith(int index, ReadOnlySpan<char> substr) => StartsWith(span.Slice(index), substr);

        /// <summary>
        /// Check if the text ends with some character.
        /// </summary>
        public bool EndsWith(char chr) => Length > 0 && span[Length - 1] == chr;
        /// <summary>
        /// Check if the text ends with some substring.
        /// </summary>
        public bool EndsWith(ReadOnlySpan<char> substr)
        {
            if (substr.Length > span.Length)
                return false;

            int start = span.Length - substr.Length;
            for (int i = 0; i < substr.Length; i++)
            {
                if (span[start + i] != substr[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Find the first index of a character.
        /// </summary>
        public int FirstIndexOf(char chr) => FirstIndexOf(0, chr);
        /// <summary>
        /// Find the first index of a character from some index onwards.
        /// </summary>
        public int FirstIndexOf(int index, char chr)
        {
            for (int i = index; i < span.Length; i++)
            {
                if (span[i] == chr)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// Find the first index of a substring.
        /// </summary>
        public int FirstIndexOf(ReadOnlySpan<char> substr) => FirstIndexOf(0, substr);
        /// <summary>
        /// Find the first index of a substring from some index onwards.
        /// </summary>
        public int FirstIndexOf(int index, ReadOnlySpan<char> substr)
        {
            for (int i = index; i < span.Length; i++)
            {
                if (StartsWith(span.Slice(i), substr))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Check if the text contains some character.
        /// </summary>
        public bool Contains(char chr) => FirstIndexOf(chr) != -1;
        /// <summary>
        /// Check if the text contains some substring.
        /// </summary>
        public bool Contains(ReadOnlySpan<char> substr) => FirstIndexOf(substr) != -1;

        /// <summary>
        /// Check if the text equals a character.
        /// </summary>
        public bool Equals(char chr) => Length == 1 && span[0] == chr;
        /// <summary>
        /// Check if the text equals another text.
        /// </summary>
        public bool Equals(ReadOnlySpan<char> span)
        {
            if (span.Length != this.span.Length)
                return false;
            for (int i = 0; i < Length; i++)
            {
                if (span[i] != this.span[i])
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Check if the text equals a string.
        /// </summary>
        public bool Equals(string str) => Equals(str.AsSpan());

        /* Private methods. */
        /// <summary>
        /// Check if a span starts with the contents of some other span.
        /// </summary>
        private static bool StartsWith(ReadOnlySpan<char> str, ReadOnlySpan<char> substr)
        {
            if (str.Length < substr.Length)
                return false;

            for (int i = 0; i < substr.Length; i++)
            {
                if (str[i] != substr[i])
                    return false;
            }

            return true;
        }
    }
}