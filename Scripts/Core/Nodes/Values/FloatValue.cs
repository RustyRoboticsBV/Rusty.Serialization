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
            string sign = negative ? "-" : "";

            // Integers.
            if (exponent == 0)
                return sign + mantissa.ToString(CultureInfo.InvariantCulture) + ".0";
            
            // Fractionals.
            string digits = mantissa.ToString(CultureInfo.InvariantCulture);

            int sciExp = digits.Length - exponent - 1;

            if (sciExp <= -4 || sciExp >= digits.Length)
            {
                string sciMantissa = digits.Length == 1 ? digits : digits[0] + "." + digits.Substring(1);
                return sign + sciMantissa + "e" + sciExp.ToString(CultureInfo.InvariantCulture);
            }

            if (digits.Length <= exponent)
                digits = digits.PadLeft(exponent + 1, '0');

            int point = digits.Length - exponent;
            return sign + digits.Substring(0, point) + "." + digits.Substring(point);
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

            // Figure out format.
            int pointIndex = span.IndexOf('.');
            int exponentIndex = span.IndexOf('e');
            if (pointIndex >= 0 && exponentIndex >= 0 && exponentIndex < pointIndex)
                throw new FormatException($"Encountered e before decimal point in float value '{new string(span)}'.");

            // Split integer part.
            ReadOnlySpan<char> integer;
            if (pointIndex == 0)
                integer = "0";
            else if (pointIndex >= 0)
                integer = span.Slice(0, pointIndex);
            else if (exponentIndex >= 0)
                integer = span.Slice(0, exponentIndex);
            else
                integer = span;

            // Split fractional part.
            ReadOnlySpan<char> fractional;
            if (pointIndex >= 0 && (pointIndex == span.Length - 1 || pointIndex == exponentIndex - 1))
                fractional = "0";
            else if (exponentIndex >= 0)
                fractional = span.Slice(pointIndex + 1, exponentIndex - (pointIndex + 1));
            else if (pointIndex >= 0)
                fractional = span.Slice(pointIndex + 1);
            else
                fractional = "0";

            // Split scientific exponent part.
            ReadOnlySpan<char> exponent;
            if (exponentIndex >= 0)
                exponent = span.Slice(exponentIndex + 1);
            else
                exponent = "0";

            // Figure out sign.
            bool negative = integer.StartsWith("-");
            if (negative)
                integer = integer.Slice(1);

            // Parse.
            int scale = fractional.Length - int.Parse(exponent);

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

        private static double RoundToSignificantDigits(double value, int significantDigits)
        {
            if (value == 0)
                return 0;

            // Find the scale to round to
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            return Math.Round(value / scale, significantDigits) * scale;
        }
    }
}