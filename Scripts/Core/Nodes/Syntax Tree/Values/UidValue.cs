using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A UID value.
    /// </summary>
    public readonly struct UidValue : IEquatable<UidValue>
    {
        /* Fields */
        public readonly Guid value;

        /* Constructors */
        public UidValue(Guid value)
        {
            this.value = value;
        }

        /* Public properties. */
        public static UidValue Empty => new UidValue(Guid.Empty);

        /* Conversion operators */
        public static implicit operator UidValue(Guid value) => new UidValue(value);
        public static implicit operator Guid(UidValue value) => value.value;

        /* Comparison operators. */
        public static bool operator ==(UidValue a, UidValue b) => a.value == b.value;
        public static bool operator !=(UidValue a, UidValue b) => a.value != b.value;

        /* Public methods */
        public override string ToString() => value.ToString();
        public override int GetHashCode() => value.GetHashCode();
        public override bool Equals(object obj) => obj is UidValue other && Equals(other);
        public bool Equals(UidValue other) => value.Equals(other.value);

        /// <summary>
        /// Parse a UID string.
        /// </summary>
        public static UidValue Parse(string text) => Parse(text.AsSpan());

        /// <summary>
        /// Parse a UID string.
        /// </summary>
        public static UidValue Parse(ReadOnlySpan<char> text) => Guid.Parse(text);
    }
}