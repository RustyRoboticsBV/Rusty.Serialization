using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents a string of bytes.
    /// </summary>
    public readonly struct BytesString : IEquatable<BytesString>
    {
        /* Fields. */
        private static readonly char[] HexLookup = "0123456789ABCDEF".ToCharArray();

        private readonly string value;

        /* Public properties. */
        public int Length => value?.Length ?? 0;

        /* Constructors. */
        private BytesString(string value) => this.value = value;

        /* Public methods. */
        public override string ToString() => value ?? "";
        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(value ?? "");
        public override bool Equals(object obj) => obj is BytesString str && Equals(str);
        public bool Equals(BytesString other) => string.Equals(value, other.value, StringComparison.OrdinalIgnoreCase);

        /* Conversion operators. */
        public static implicit operator BytesString(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length % 2 != 0)
                throw new ArgumentException("Hex string length must be even.");

            for (int i = 0; i < value.Length; i++)
            {
                if (!(value[i] >= '0' && value[i] <= '9' || value[i] >= 'A' && value[i] <= 'F' || value[i] >= 'a' && value[i] <= 'f'))
                    throw new ArgumentException($"Invalid bytes string {value}.");
            }

            return new BytesString(value);
        }

        public static implicit operator BytesString(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            int len = value.Length;
            if (len == 0)
                return new BytesString(string.Empty);

            char[] buffer = new char[len * 2];

            for (int i = 0; i < len; i++)
            {
                byte b = value[i];
                int idx = i * 2;

                buffer[idx] = HexLookup[b >> 4];
                buffer[idx + 1] = HexLookup[b & 0xF];
            }

            return new string(buffer);
        }

        public static implicit operator string(BytesString value) => value.ToString();

        public static implicit operator byte[](BytesString value)
        {
            if (value.Length % 2 != 0)
                throw new FormatException("Hex string length must be even.");

            string str = value.value ?? "";
            if (str.Length == 0)
                return Array.Empty<byte>();

            byte[] result = new byte[str.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                int hi = FromHexChar(value.value[i * 2]);
                int lo = FromHexChar(value.value[i * 2 + 1]);
                result[i] = (byte)((hi << 4) | lo);
            }

            return result;
        }

        /* Private methods. */
        private static int FromHexChar(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';

            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;

            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            throw new FormatException($"Invalid hex character: '{c}'");
        }
    }
}