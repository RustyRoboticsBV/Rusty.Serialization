using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents a single Unicode character.
    /// </summary>
    public readonly struct CharString : IEquatable<CharString>
    {
        /* Fields. */
        private readonly string value;

        /* Constructors. */
        private CharString(string value) => this.value = value;

        /* Public properties. */
        public bool IsSurrogate => value != null && value.Length == 2;

        /* Public methods. */
        public override string ToString() => value ?? string.Empty;
        public override int GetHashCode() => value?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => obj is CharString cs && Equals(cs);
        public bool Equals(CharString other) => value == other.value;

        /* Conversion operators. */
        public static implicit operator CharString(char c)
        {
            return new CharString(c.ToString());
        }

        public static implicit operator CharString(string str)
        {
            if (str is null)
                throw new ArgumentNullException(nameof(str));

            if (str.Length == 1)
                return new CharString(str);

            if (str.Length == 2 && char.IsSurrogatePair(str[0], str[1]))
                return new CharString(str);

            throw new FormatException("String must be a single character or a valid surrogate pair.");
        }

        public static implicit operator string(CharString value) => value.ToString();

        public static implicit operator char(CharString value)
        {
            if (value.value == null || value.value.Length == 0)
                throw new InvalidOperationException("CharString is empty.");

            if (value.value.Length == 1)
                return value.value[0];

            throw new InvalidOperationException("Cannot convert a surrogate pair to a single char.");
        }
    }
}
