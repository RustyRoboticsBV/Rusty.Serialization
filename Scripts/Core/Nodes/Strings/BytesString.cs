using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents a string of bytes in Base64.
    /// </summary>
    public readonly struct BytesString : IEquatable<BytesString>
    {
        /* Fields. */
        private readonly string value;

        /* Public properties. */
        public int Length => value?.Length ?? 0;

        /* Constructors. */
        private BytesString(string value) => this.value = value;

        /* Public methods. */
        public override string ToString() => value ?? "";
        public override int GetHashCode() => value.GetHashCode();
        public override bool Equals(object obj) => obj is BytesString str && Equals(str);
        public bool Equals(BytesString other) => string.Equals(value, other.value);

        /* Conversion operators. */
        public static implicit operator BytesString(string value)
        {
            // Handle null.
            value = value ?? "";

            // Validate.
            try
            {
                Convert.FromBase64String(value);
            }
            catch
            {
                throw new FormatException($"Not a valid Base64 string: {value}");
            }

            // Create new bytes string.
            return new BytesString(value);
        }

        public static implicit operator BytesString(byte[] value)
        {
            if (value == null || value.Length == 0)
                return new BytesString(string.Empty);
            return new BytesString(Convert.ToBase64String(value));
        }

        public static implicit operator string(BytesString value) => value.ToString();

        public static implicit operator byte[](BytesString value)
        {
            if (string.IsNullOrEmpty(value.value))
                return Array.Empty<byte>();
            try
            {
                return Convert.FromBase64String(value.value);
            }
            catch
            {
                throw new FormatException($"Invalid Base64 string: {value.value}");
            }
        }
    }
}