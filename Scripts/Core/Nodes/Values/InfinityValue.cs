using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An infinity value.
    /// </summary>
    public readonly struct InfinityValue : IEquatable<InfinityValue>
    {
        /* Fields */
        public readonly bool positive;

        /* Constructors */
        public InfinityValue(bool positive)
        {
            this.positive = positive;
        }

        /* Conversion operators. */
        public static implicit operator bool(InfinityValue value) => value.positive;
        public static implicit operator InfinityValue(bool positive) => new InfinityValue(positive);

        /* Comparison operators */
        public static bool operator ==(InfinityValue a, InfinityValue b) => a.Equals(b);
        public static bool operator !=(InfinityValue a, InfinityValue b) => !a.Equals(b);

        /* Public methods */
        public override string ToString()
        {
            return new string(AsSpan());
        }

        public ReadOnlySpan<char> AsSpan() => positive ? "Infinity" : "-Infinity";
        public override int GetHashCode() => positive.GetHashCode();
        public override bool Equals(object obj) => obj is InfinityValue other && Equals(other);
        public bool Equals(InfinityValue other) => positive == other.positive;

        /// <summary>
        /// Parse a string into an infinity value.
        /// </summary>
        public static InfinityValue Parse(string str) => Parse(str.AsSpan());

        /// <summary>
        /// Parse a character span into an infinity value.
        /// </summary>
        public static InfinityValue Parse(ReadOnlySpan<char> span)
        {
            span = span.Trim();
            if (span.SequenceEqual("Infinity"))
                return new InfinityValue(true);
            if (span.SequenceEqual("-Infinity"))
                return new InfinityValue(false);
            throw new FormatException($"Cannot parse string '{new string(span)}' as an infinity.");
        }
    }
}
