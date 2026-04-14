using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string value.
    /// </summary>
    public readonly struct StringValue : IEquatable<StringValue>
    {
        /* Fields */
        public readonly string value;

        /* Constructors */
        public StringValue(string value)
        {
            this.value = value;
        }

        /* Conversion operators */
        public static implicit operator StringValue(string value) => new StringValue(value);
        public static explicit operator string(StringValue value) => value.value;

        /* Public methods */
        public override string ToString() => value.ToString();
        public override int GetHashCode() => value.GetHashCode();
        public override bool Equals(object obj) => obj is StringValue other && Equals(other);
        public bool Equals(StringValue other) => value.Equals(other.value);

        /// <summary>
        /// Parse an integer string.
        /// </summary>
        public static StringValue Parse(ReadOnlySpan<char> text)
        {
            return new StringValue(new string(text));
        }
    }
}