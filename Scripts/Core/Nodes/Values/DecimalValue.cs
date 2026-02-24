using System;
using System.Numerics;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A decimal value with arbitrary precision.
    /// </summary>
    public readonly struct DecimalValue : IEquatable<DecimalValue>
    {
        /* Fields */
        public readonly bool negative;
        public readonly BigInteger mantissa;
        public readonly int scale;

        private readonly static BigInteger DecimalBitRange = BigInteger.One << 96;

        /* Public properties. */
        public bool IsFractional => scale > 0;

        /* Constructors */
        public DecimalValue(decimal value)
        {
            int[] bits = decimal.GetBits(value);

            int low = bits[0];
            int mid = bits[1];
            int high = bits[2];
            int signScale = bits[3];

            mantissa = new BigInteger(low)
             | (new BigInteger(mid) << 32)
             | (new BigInteger(high) << 64);

            negative = (signScale & 0x80000000) != 0;

            scale = (signScale >> 16) & 0xFF;
        }

        public DecimalValue(bool negative, BigInteger mantissa, int scale)
        {
            if (mantissa < 0)
                throw new ArgumentOutOfRangeException(nameof(mantissa), "May not be negative.");
            if (scale < 0)
                throw new ArgumentOutOfRangeException(nameof(scale), "May not be negative.");
            this.negative = negative;
            this.mantissa = mantissa;
            this.scale = scale;
        }

        /* Conversion operators. */
        public static implicit operator DecimalValue(BigInteger value) => new DecimalValue(value < 0, BigInteger.Abs(value), 0);
        public static implicit operator DecimalValue(decimal value) => new DecimalValue(value);
        public static explicit operator decimal(DecimalValue value)
        {
            if (value.scale > 28)
                throw new OverflowException("Scale out of range for decimal.");

            BigInteger abs = BigInteger.Abs(value.mantissa);

            if (abs > (DecimalBitRange) - 1)
                throw new OverflowException("Value out of range for decimal.");

            int lo = (int)(abs & 0xFFFFFFFF);
            int mid = (int)((abs >> 32) & 0xFFFFFFFF);
            int hi = (int)((abs >> 64) & 0xFFFFFFFF);

            return new decimal(lo, mid, hi, value.negative, (byte)value.scale);
        }

        /* Public methods. */
        public override string ToString()
        {
            // Handle integers.
            if (scale == 0)
            {
                if (negative)
                    return "-" + mantissa.ToString(CultureInfo.InvariantCulture);
                else
                    return mantissa.ToString(CultureInfo.InvariantCulture);
            }

            // Handle fractionals.
            BigInteger absMantissa = BigInteger.Abs(mantissa);
            string digits = absMantissa.ToString(CultureInfo.InvariantCulture);

            // Pad with leading zeros if necessary.
            if (digits.Length <= scale)
                digits = digits.PadLeft(scale + 1, '0');

            int decimalPos = digits.Length - scale;
            string integerPart = digits.Substring(0, decimalPos);
            string fractionalPart = digits.Substring(decimalPos);

            // Ensure integer part is not empty.
            if (string.IsNullOrEmpty(integerPart))
                integerPart = "0";

            string result = integerPart + "." + fractionalPart;

            if (mantissa < 0)
                result = "-" + result;

            return result;
        }

        public override int GetHashCode() => HashCode.Combine(mantissa, scale);
        public override bool Equals(object obj) => obj is DecimalValue other && Equals(other);
        public bool Equals(DecimalValue other) => mantissa.Equals(other.mantissa) && scale.Equals(other.scale);

        /// <summary>
        /// Try to parse a string as a decimal number.
        /// </summary>
        public static DecimalValue Parse(string str)
        {
            // Get trimmed string.
            str = str.Trim();
            ReadOnlySpan<char> span = str.AsSpan();
            return Parse(span);
        }

        /// <summary>
        /// Try to parse a read-only span as a decimal number.
        /// </summary>
        public static DecimalValue Parse(ReadOnlySpan<char> span)
        {
            // Find decimal point.
            int pointIndex = span.IndexOf('.');

            // Handle integers.
            if (pointIndex == -1)
                return new DecimalValue(span.StartsWith("-"), BigInteger.Parse(span), 0);

            // Handle fractionals.
            bool negative = span.StartsWith("-");
            if (negative)
            {
                span = span.Slice(1);
                pointIndex--;
            }

            ReadOnlySpan<char> integer = span.Slice(0, pointIndex);
            if (integer.Length == 0)
                integer = "0";

            ReadOnlySpan<char> fractional = span.Slice(pointIndex + 1);


            int scale = fractional.Length;

            if (scale == 0)
                throw new FormatException("Fractional part may not be empty.");
            else
                UnityEngine.Debug.Log(scale);

            int length = integer.Length + fractional.Length;
            BigInteger mantissa;
            if (length < 1024)
            {
                Span<char> mantissaStr = stackalloc char[length];
                integer.CopyTo(mantissaStr);
                fractional.CopyTo(mantissaStr.Slice(integer.Length));
                try
                {
                    mantissa = BigInteger.Parse(mantissaStr);
                }
                catch
                {
                    throw new FormatException($"Cannot parse mantissa \"{new string(mantissaStr)}\"");
                }
            }
            else
            {
                char[] mantissaChars = new char[length];
                integer.CopyTo(mantissaChars.AsSpan(0, integer.Length));
                fractional.CopyTo(mantissaChars.AsSpan(integer.Length, fractional.Length));
                mantissa = BigInteger.Parse(mantissaChars);
            }
            return new DecimalValue(negative, BigInteger.Abs(mantissa), scale);
        }
    }
}