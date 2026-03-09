using System;
using System.Numerics;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A boolean array value.
    /// </summary>
    public readonly struct BitmaskValue : IEquatable<BitmaskValue>
    {
        /* Fields */
        public readonly bool[] value;

        /* Public properties */
        public bool IsEmpty => Length == 0;
        public int Length => value.Length;

        /* Constructors */
        public BitmaskValue(bool[] value)
        {
            this.value = value;
        }

        public BitmaskValue(int value)
        {
            this.value = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                this.value[i] = (value & (1 << i)) != 0;
            }
        }

        /* Conversion operators */
        public static implicit operator BitmaskValue(bool[] value) => new BitmaskValue(value);
        public static implicit operator bool[](BitmaskValue value) => value.value;
        public static implicit operator BitmaskValue(int value) => new BitmaskValue(value);
        public static implicit operator int(BitmaskValue value)
        {
            int result = 0;
            for (int i = 0; i < value.value.Length && i < 32; i++)
            {
                if (value.value[i])
                    result += 1 << i;
            }
            return result;
        }

        /* Comparison operators */
        public static bool operator ==(BitmaskValue a, BitmaskValue b) => a.Equals(b);
        public static bool operator !=(BitmaskValue a, BitmaskValue b) => !a.Equals(b);

        /* Public methods */
        public override string ToString()
        {
            if (value == null || value.Length == 0)
                return string.Empty;
            else
            {
                Span<char> span = stackalloc char[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    span[i] = value[value.Length - 1 - i] ? '1' : '0';
                }
                return new string(span);
            }
        }

        public override int GetHashCode()
        {
            if (value == null)
                return 0;

            HashCode hash = new HashCode();
            for (int i = 0; i < value.Length; i++)
            {
                hash.Add(value[i]);
            }
            return hash.ToHashCode();
        }

        public override bool Equals(object obj) => obj is BitmaskValue other && Equals(other);

        public bool Equals(BitmaskValue other)
        {
            if (Length != other.Length)
                return false;

            for (int i = 0; i < Length; i++)
            {
                if (value[i] != other.value[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Parse a bitstring into a bitmask value.
        /// </summary>
        public static BitmaskValue Parse(string str) => Parse(str.AsSpan());

        /// <summary>
        /// Parse a bit character span into a bitmask value.
        /// </summary>
        public static BitmaskValue Parse(ReadOnlySpan<char> span)
        {
            span = span.Trim();

            if (span.Length == 0)
                return new BitmaskValue(Array.Empty<bool>());

            try
            {
                bool[] data = new bool[span.Length];
                for (int i = 0; i < span.Length; i++)
                {
                    if (span[i] == '1')
                        data[span.Length - 1 - i] = true;
                    else if (span[i] == '0')
                        data[span.Length - 1 - i] = false;
                    else
                        throw new Exception();
                }
                return new BitmaskValue(data);
            }
            catch (FormatException)
            {
                throw new FormatException($"Not a valid bitmask: {new string(span)}.");
            }
        }
    }
}