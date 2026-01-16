using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents an integer number.
    /// </summary>
    public readonly struct IntString : IEquatable<IntString>, IEquatable<string>
    {
        /* Fields. */
        private readonly string value;

        /* Public properties. */
        public bool IsNegative => value?.StartsWith('-') ?? false;
        public bool IsZero => value == "0" || value == "-0";
        public bool IsOne => value == "1";

        /* Constructors. */
        private IntString(string value) => this.value = value;

        /* Public methods. */
        public override string ToString() => value ?? "0";
        public override int GetHashCode() => value?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => obj is IntString str && Equals(str);
        public bool Equals(IntString other) => value == other.value;
        public bool Equals(string str) => value == str;

        /* Conversion operators. */
        public static implicit operator IntString(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int length = value.Length;
            if (length == 0)
                throw new FormatException("Empty string is not a valid integer number.");

            int i = 0;

            if (value[i] == '-')
            {
                i++;
                if (i == length)
                    throw new FormatException("Invalid integer number string.");
            }

            while (i < length && value[i] >= '0' && value[i] <= '9')
            {
                i++;
            }

            if (i != length)
                throw new FormatException("Invalid integer number string.");

            return new IntString(value);
        }

        public static implicit operator IntString(byte value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(sbyte value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(short value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(ushort value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(int value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(long value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(uint value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(ulong value)
        {
            return new IntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator IntString(UnsignedIntString value)
        {
            return new IntString(value.ToString());
        }

        public static implicit operator IntString(RealString value)
        {
            string str = value;
            int dotIndex = str.IndexOf('.');
            if (dotIndex == -1)
                return str;
            else
                return str.Substring(0, dotIndex);
        }

        public static implicit operator IntString(UnsignedRealString value)
        {
            string str = value;
            int dotIndex = str.IndexOf('.');
            if (dotIndex == -1)
                return str;
            else
                return str.Substring(0, dotIndex);
        }

        public static implicit operator string(IntString value) => value.ToString();
    }
}