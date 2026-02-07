using System;
using System.Numerics;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A floating-point decimal value with arbitrary precision. It does not preserve trailing zeros.
    /// </summary>
    public readonly struct FloatValue : IEquatable<FloatValue>
    {
        /* Fields */
        public readonly bool negative;
        public readonly BigInteger mantissa;
        public readonly int exponent;

        /* Public properties */
        public bool IsFractional => exponent < 0;

        /* Constructors */
        public FloatValue(float value) : this((double)value) { }

        public FloatValue(double value)
        {
            long bits = BitConverter.DoubleToInt64Bits(value);

            bool negative = (bits & (1L << 63)) != 0;
            int rawExp = (int)((bits >> 52) & 0x7FF);
            long rawMantissa = bits & ((1L << 52) - 1);

            // Zero (includes negative zero)
            if (rawExp == 0 && rawMantissa == 0)
            {
                this.negative = negative; // preserve sign
                this.mantissa = BigInteger.Zero;
                this.exponent = 0;
                return;
            }

            // NaN / Infinity
            if (rawExp == 0x7FF)
                throw new ArgumentException("NaN and Infinity are not supported.", nameof(value));

            const int Bias = 1023;

            BigInteger mantissa;
            int exp2;

            if (rawExp == 0)
            {
                // Subnormal
                mantissa = new BigInteger(rawMantissa);
                exp2 = 1 - Bias - 52;
            }
            else
            {
                // Normalized
                mantissa = new BigInteger(rawMantissa | (1L << 52));
                exp2 = rawExp - Bias - 52;
            }

            int exponent;

            if (exp2 >= 0)
            {
                mantissa <<= exp2;
                exponent = 0;
            }
            else
            {
                int shift = -exp2;
                mantissa *= BigInteger.Pow(5, shift);
                exponent = shift;
            }

            Normalize(negative, mantissa, exponent, out this.negative, out this.mantissa, out this.exponent);
        }

        public FloatValue(bool negative, BigInteger mantissa, int exponent)
        {
            if (mantissa < 0)
                throw new ArgumentOutOfRangeException(nameof(mantissa));

            Normalize(negative, mantissa, exponent, out this.negative, out this.mantissa, out this.exponent);
        }

        /* Conversion operators */
        public static implicit operator FloatValue(float value) => new FloatValue(value);
        public static implicit operator FloatValue(double value) => new FloatValue(value);

        public static explicit operator float(FloatValue value) => (float)(double)value;
        public static explicit operator double(FloatValue value) => (double)value.mantissa * Math.Pow(10, -value.exponent);

        /* Compatison operators. */
        public static bool operator ==(FloatValue a, FloatValue b) => a.Equals(b);
        public static bool operator !=(FloatValue a, FloatValue b) => !a.Equals(b);

        /* Public methods. */
        public override string ToString()
        {
            // Integers.
            if (exponent == 0)
                return (negative ? "-" : "") + mantissa.ToString(CultureInfo.InvariantCulture) + ".0";
            
            // Fractionals.
            string digits = mantissa.ToString(CultureInfo.InvariantCulture);

            if (digits.Length <= exponent)
                digits = digits.PadLeft(exponent + 1, '0');

            int point = digits.Length - exponent;
            string result = digits.Substring(0, point) + "." + digits.Substring(point);

            return negative ? "-" + result : result;
        }

        public override int GetHashCode() => HashCode.Combine(mantissa, exponent);

        public override bool Equals(object obj) => obj is FloatValue other && Equals(other);

        public bool Equals(FloatValue other)
            => mantissa.Equals(other.mantissa) && exponent == other.exponent;

        public static FloatValue Parse(string str)
            => Parse(str.AsSpan());

        public static FloatValue Parse(ReadOnlySpan<char> span)
        {
            span = span.Trim();

            int pointIndex = span.IndexOf('.');

            if (pointIndex == -1)
            {
                bool neg = span.StartsWith("-");
                BigInteger m = BigInteger.Parse(span);
                return new FloatValue(neg, BigInteger.Abs(m), 0);
            }

            ReadOnlySpan<char> integer = span.Slice(0, pointIndex);
            ReadOnlySpan<char> fractional = span.Slice(pointIndex + 1);

            bool negative = integer.StartsWith("-");
            if (negative)
                integer = integer.Slice(1);

            int scale = fractional.Length;

            if (scale == 0)
                return new FloatValue(negative, BigInteger.Parse(integer), 0);

            int length = integer.Length + fractional.Length;
            Span<char> buffer = length < 1024
                ? stackalloc char[length]
                : new char[length];

            integer.CopyTo(buffer);
            fractional.CopyTo(buffer.Slice(integer.Length));

            BigInteger mantissa = BigInteger.Parse(buffer);
            return new FloatValue(negative, mantissa, scale);
        }

        /* Private methods. */
        private static void Normalize(bool negative, BigInteger mantissa, int exponent,
            out bool normNegative, out BigInteger normMantissa, out int normExponent)
        {
            if (mantissa.IsZero)
            {
                normNegative = negative;
                normMantissa = BigInteger.Zero;
                normExponent = 0;
                return;
            }

            BigInteger ten = new BigInteger(10);

            while (exponent > 0)
            {
                BigInteger remainder;
                BigInteger div = BigInteger.DivRem(mantissa, ten, out remainder);
                if (!remainder.IsZero)
                    break;

                mantissa = div;
                exponent--;
            }

            normNegative = negative;
            normMantissa = mantissa;
            normExponent = exponent;
        }
    }
}