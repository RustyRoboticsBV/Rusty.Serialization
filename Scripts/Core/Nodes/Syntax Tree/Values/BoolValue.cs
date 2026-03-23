using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A boolean value.
    /// </summary>
    public readonly struct BoolValue : IEquatable<BoolValue>
    {
        /* Fields */
        public readonly bool value;

        /* Constructors */
        public BoolValue(bool value)
        {
            this.value = value;
        }

        /* Conversion operators */
        public static implicit operator BoolValue(bool value) => new BoolValue(value);
        public static implicit operator bool(BoolValue value) => value.value;

        /* Comparison operators. */
        public static bool operator ==(BoolValue a, BoolValue b) => a.value == b.value;
        public static bool operator !=(BoolValue a, BoolValue b) => a.value != b.value;

        /* Public methods */
        public override string ToString() => value ? "true" : "false";
        public override int GetHashCode() => value.GetHashCode();
        public override bool Equals(object obj) => obj is BoolValue other && Equals(other);
        public bool Equals(BoolValue other) => value.Equals(other.value);


#if NET_STANDARD_2_1
        /// <summary>
        /// Parse a boolean string.
        /// </summary>
        public static BoolValue Parse(string text) => Parse(text.AsSpan());

        /// <summary>
        /// Parse a boolean string.
        /// </summary>
        public static BoolValue Parse(ReadOnlySpan<char> text)
        {
            if (text.SequenceEqual("true"))
                return true;
            if (text.SequenceEqual("false"))
                return false;
            throw new FormatException($"Cannot parse '{new string(text)}' as a boolean.");
        }
#else
        /// <summary>
        /// Parse a boolean string.
        /// </summary>
        public static BoolValue Parse(string text)
        {
            if (text == "true")
                return true;
            if (text == "false")
                return false;
            throw new FormatException($"Cannot parse '{text}' as a boolean.");
        }
#endif
    }
}