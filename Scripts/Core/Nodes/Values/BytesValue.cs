using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A byte array value.
    /// </summary>
    public readonly struct BytesValue : IEquatable<BytesValue>
    {
        /* Fields */
        public readonly byte[] value;

        /* Public properties */
        public bool IsEmpty => value == null || value.Length == 0;
        public int Length => value?.Length ?? 0;

        /* Constructors */
        public BytesValue(byte[] value)
        {
            this.value = value ?? Array.Empty<byte>();
        }

        /* Conversion operators */
        public static implicit operator BytesValue(byte[] value) => new BytesValue(value);
        public static implicit operator byte[](BytesValue value) => value.value;

        /* Comparison operators */
        public static bool operator ==(BytesValue a, BytesValue b) => a.Equals(b);
        public static bool operator !=(BytesValue a, BytesValue b) => !a.Equals(b);

        /* Public methods */
        public override string ToString()
        {
            if (value == null || value.Length == 0)
                return string.Empty;
            else
                return Convert.ToBase64String(value);
        }

        public override int GetHashCode()
        {
            if (value == null)
                return 0;

            HashCode hash = new HashCode();
            foreach (byte b in value)
            {
                hash.Add(b);
            }
            return hash.ToHashCode();
        }

        public override bool Equals(object obj) => obj is BytesValue other && Equals(other);

        public bool Equals(BytesValue other)
        {
            if (ReferenceEquals(value, other.value))
                return true;

            if (value == null || other.value == null)
                return false;

            if (value.Length != other.value.Length)
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != other.value[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Parse a Base64-encoded string into a byte array value.
        /// </summary>
        public static BytesValue Parse(string str) => Parse(str.AsSpan());

        /// <summary>
        /// Parse a Base64-encoded character span into a byte array value.
        /// </summary>
        public static BytesValue Parse(ReadOnlySpan<char> span)
        {
            span = span.Trim();

            if (span.Length == 0)
                return new BytesValue(Array.Empty<byte>());

            try
            {
                byte[] data = Convert.FromBase64String(span.ToString());
                return new BytesValue(data);
            }
            catch (FormatException)
            {
                throw new FormatException($"Not a valid Base64 byte sequence: {new string(span)}.");
            }
        }
    }
}
