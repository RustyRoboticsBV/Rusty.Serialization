using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents a real number.
    /// </summary>
    public readonly struct RealString : IEquatable<RealString>
    {
        /* Constants. */
        private const string format = "0.#############################";

        /* Fields. */
        private readonly string value;

        /* Public properties. */
        public bool IsNegative => value?.StartsWith('-') ?? false;
        public bool IsIntegral => value != null && !value.Contains('.');
        public bool IsFractional => value?.Contains('.') ?? false;
        public bool IsZero
        {
            get
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != '-' && value[i] != '0' && value[i] != '.')
                        return false;
                }
                return true;
            }
        }

        /* Constructors. */
        private RealString(string value) => this.value = value;

        /* Public methods. */
        public override string ToString() => value ?? "0";
        public override int GetHashCode() => value?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => obj is RealString str && Equals(str);
        public bool Equals(RealString other) => value == other.value;

        /* Conversion operators. */
        public static implicit operator RealString(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int length = value.Length;
            if (length == 0)
                throw new FormatException("Empty string is not a valid real number.");

            int i = 0;

            if (value[i] == '-')
            {
                i++;
                if (i == length)
                    throw new FormatException($"Invalid real number string: {value}.");
            }

            while (i < length && value[i] >= '0' && value[i] <= '9')
            {
                i++;
            }

            if (i < length)
            {
                if (value[i] != '.')
                    throw new FormatException($"Invalid real number string: {value}.");

                i++;

                while (i < length && value[i] >= '0' && value[i] <= '9')
                {
                    i++;
                }
            }

            if (i != length)
                throw new FormatException($"Invalid real number string: {value}.");

            return new RealString(value);
        }

        public static implicit operator RealString(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || float.IsNegativeInfinity(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            return new RealString(value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static implicit operator RealString(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || double.IsNegativeInfinity(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            return new RealString(value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static implicit operator RealString(decimal value)
        {
            return new RealString(value.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator RealString(IntString value)
        {
            return new RealString(value);
        }

        public static implicit operator RealString(UnsignedIntString value)
        {
            return new RealString(value);
        }

        public static implicit operator RealString(UnsignedRealString value)
        {
            return new RealString(value);
        }

        public static implicit operator string(RealString value) => value.ToString();
    }
}