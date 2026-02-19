using System;
using System.Numerics;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A representation of a floating-point value with arbitrary precision, in the format [-]{integral}.{fractional}e{exponent}.
    /// </summary>
    public readonly struct FloatValue : IEquatable<FloatValue>
    {
        /* Fields */
        public readonly bool negative;
        public readonly BigInteger integral;
        public readonly string fractional;
        public readonly BigInteger exponent;

        /* Constructors */
        public FloatValue(float value)
        {
            FloatValue val = Parse(value.ToString(CultureInfo.InvariantCulture));
            negative = val.negative;
            integral = val.integral;
            fractional = val.fractional;
            exponent = val.exponent;
        }

        public FloatValue(double value)
        {
            FloatValue val = Parse(value.ToString(CultureInfo.InvariantCulture));
            negative = val.negative;
            integral = val.integral;
            fractional = val.fractional;
            exponent = val.exponent;
        }

        public FloatValue(bool negative, BigInteger integral, string fractional, BigInteger exponent)
        {
            this.negative = negative;
            this.integral = integral;
            this.fractional = fractional;
            this.exponent = exponent;
        }

        /* Conversion operators */
        public static implicit operator FloatValue(float value) => new FloatValue(value);
        public static implicit operator FloatValue(double value) => new FloatValue(value);

        public static explicit operator float(FloatValue value) => float.Parse(value.ToString());
        public static explicit operator double(FloatValue value) => double.Parse(value.ToString());

        /* Compatison operators. */
        public static bool operator ==(FloatValue a, FloatValue b) => a.Equals(b);
        public static bool operator !=(FloatValue a, FloatValue b) => !a.Equals(b);

        /* Public methods. */
        public override string ToString()
        {
            string sign = negative ? "-" : "";
            if (exponent != 0)
                return $"{sign}{integral}.{fractional}e{exponent}";
            else
                return $"{sign}{integral}.{fractional}";
        }

        public override int GetHashCode() => HashCode.Combine(negative, integral, fractional, exponent);

        public override bool Equals(object obj) => obj is FloatValue other && Equals(other);

        public bool Equals(FloatValue other) => negative == other.negative && integral == other.integral
            && fractional == other.fractional && exponent == other.exponent;

        public static FloatValue Parse(string str) => Parse(str.AsSpan());

        public static FloatValue Parse(ReadOnlySpan<char> span)
        {
            span = span.Trim();
            if (span.IsEmpty)
                throw new FormatException("Empty float literal.");

            // Sign.
            bool negative = false;
            if (span[0] == '-')
            {
                negative = true;
                span = span.Slice(1);
            }
            else if (span[0] == '+')
            {
                span = span.Slice(1);
            }

            // Exponent.
            int expIndex = span.IndexOfAny('e', 'E');

            ReadOnlySpan<char> significand;
            ReadOnlySpan<char> exponentPart;

            if (expIndex >= 0)
            {
                significand = span.Slice(0, expIndex);
                exponentPart = span.Slice(expIndex + 1);
                if (exponentPart.IsEmpty)
                    throw new FormatException("Missing exponent digits.");
            }
            else
            {
                significand = span;
                exponentPart = "0";
            }

            // Decimal point.
            int pointIndex = significand.IndexOf('.');

            ReadOnlySpan<char> integralPart;
            ReadOnlySpan<char> fractionalPart;

            if (pointIndex >= 0)
            {
                integralPart = pointIndex == 0 ? "0" : significand.Slice(0, pointIndex);
                fractionalPart = pointIndex == significand.Length - 1 ? "0" : significand.Slice(pointIndex + 1);
            }
            else
            {
                integralPart = significand;
                fractionalPart = "0";
            }

            // Fix empty integer/fractional part.
            if (integralPart.IsEmpty)
                integralPart = "0";

            if (fractionalPart.IsEmpty)
                fractionalPart = "0";

            // Parse components.
            BigInteger integral = BigInteger.Parse(integralPart);
            BigInteger exponent = BigInteger.Parse(exponentPart);
            string fractional = fractionalPart.ToString();

            return new FloatValue(negative, integral, fractional, exponent);
        }

    }
}