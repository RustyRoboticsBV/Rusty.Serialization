using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents an integer number.
    /// </summary>
    public readonly struct UnsignedIntString : IEquatable<UnsignedIntString>
    {
        /* Fields. */
        private readonly string value;

        /* Public properties. */
        public bool IsZero => value == "0";
        public bool IsOne => value == "1";

        /* Constructors. */
        private UnsignedIntString(string value) => this.value = value;

        /* Public methods. */
        public override string ToString() => value ?? "0";
        public override int GetHashCode() => value?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => obj is IntString str && Equals(str);
        public bool Equals(UnsignedIntString other) => value == other.value;

        /* Conversion operators. */
        public static implicit operator UnsignedIntString(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.StartsWith('-'))
                throw new ArgumentException("Unsigned int strings may not be negative.");

            int length = value.Length;
            if (length == 0)
                throw new FormatException("Empty string is not a valid unsigned integer number.");

            int i = 0;
            while (i < length && value[i] >= '0' && value[i] <= '9')
            {
                i++;
            }

            if (i != length)
                throw new FormatException("Invalid unsigned integer number string.");

            return new UnsignedIntString(value);
        }

        public static implicit operator UnsignedIntString(byte value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(sbyte value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(short value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(ushort value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(int value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(long value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(uint value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(ulong value)
        {
            return new UnsignedIntString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedIntString(IntString value)
        {
            return new UnsignedIntString(value.ToString());
        }

        public static implicit operator UnsignedIntString(RealString value)
        {
            string str = value;
            int dotIndex = str.IndexOf('.');
            if (dotIndex == -1)
                return str;
            else
                return str.Substring(0, dotIndex);
        }

        public static implicit operator UnsignedIntString(UnsignedRealString value)
        {
            string str = value;
            int dotIndex = str.IndexOf('.');
            if (dotIndex == -1)
                return str;
            else
                return str.Substring(0, dotIndex);
        }

        public static implicit operator string(UnsignedIntString value) => value.ToString();
    }
}