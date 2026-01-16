using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents a real number.
    /// </summary>
    public readonly struct UnsignedRealString : IEquatable<UnsignedRealString>
    {
        /* Constants. */
        private const string format = "0.#############################";

        /* Fields. */
        private readonly string value;

        /* Public properties. */
        public bool IsIntegral => value != null && !value.Contains('.');
        public bool IsFractional => value?.Contains('.') ?? false;
        public bool IsZero
        {
            get
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != '0' && value[i] != '.')
                        return false;
                }
                return true;
            }
        }

        /* Constructors. */
        private UnsignedRealString(string value) => this.value = value;

        /* Public methods. */
        public override string ToString() => value ?? "0";
        public override int GetHashCode() => value?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => obj is UnsignedRealString str && Equals(str);
        public bool Equals(UnsignedRealString other) => value == other.value;

        /* Conversion operators. */
        public static implicit operator UnsignedRealString(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.StartsWith('-'))
                throw new ArgumentException("Unsigned real strings may not be negative.");

            int length = value.Length;
            if (length == 0)
                throw new FormatException("Empty string is not a valid real number.");

            int i = 0;

            while (i < length && value[i] >= '0' && value[i] <= '9')
            {
                i++;
            }

            if (i < length)
            {
                if (value[i] != '.')
                    throw new FormatException("Invalid real number string.");

                i++;

                while (i < length && value[i] >= '0' && value[i] <= '9')
                {
                    i++;
                }
            }

            if (i != length)
                throw new FormatException("Invalid real number string.");

            return new UnsignedRealString(value);
        }

        public static implicit operator UnsignedRealString(IntString value)
        {
            return new UnsignedRealString(value);
        }

        public static implicit operator UnsignedRealString(UnsignedIntString value)
        {
            return new UnsignedRealString(value);
        }

        public static implicit operator UnsignedRealString(RealString value)
        {
            return new UnsignedRealString(value);
        }

        public static implicit operator UnsignedRealString(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || float.IsNegativeInfinity(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            return new UnsignedRealString(value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedRealString(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || double.IsNegativeInfinity(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            return new UnsignedRealString(value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static implicit operator UnsignedRealString(decimal value)
        {
            return new UnsignedRealString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator string(UnsignedRealString value) => value.ToString();
    }
}