using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A color node value.
    /// </summary>
    public readonly struct ColorValue : IEquatable<ColorValue>
    {
        /* Fields */
        public readonly byte r;
        public readonly byte g;
        public readonly byte b;
        public readonly byte a;

        /* Constructors */
        public ColorValue(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /* Conversion operators */
        public static implicit operator ColorValue((byte, byte, byte, byte) value)
            => new ColorValue(value.Item1, value.Item2, value.Item3, value.Item4);

        /* Public methods */
        public override string ToString()
        {
            if (a == 255)
                return $"#{r:X2}{g:X2}{b:X2}";
            else
                return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
        }
        public override int GetHashCode() => HashCode.Combine(r, g, b, a);
        public override bool Equals(object obj) => obj is ColorValue other && Equals(other);
        public bool Equals(ColorValue other) => r == other.r && g == other.g && b == other.b && a == other.a;

        /// <summary>
        /// Try to parse a string as a color value.
        /// </summary>
        public static ColorValue Parse(string str)
        {
            // Get trimmed string.
            str = str.Trim();
            ReadOnlySpan<char> span = str.AsSpan();
            return Parse(span);
        }

        /// <summary>
        /// Try to parse a string as a color value.
        /// </summary>
        public static ColorValue Parse(ReadOnlySpan<char> str)
        {
            if (str.Length == 0)
                throw new ArgumentException("Cannot parse empty strings.");

            // Remove leading number sign.
            if (str[0] == '#')
                str = str.Slice(1);

            // Parse.
            if (str.Length != 3 && str.Length != 4 && str.Length != 6 && str.Length != 8)
                throw new FormatException($"Wrong length {new string(str)}.");

            bool shorthand = str.Length == 3 || str.Length == 4;
            bool hasAlpha = str.Length == 4 || str.Length == 8;

            byte r, g, b, a = 255;

            if (shorthand)
            {
                r = ParseHex(str.Slice(0, 1));
                g = ParseHex(str.Slice(1, 1));
                b = ParseHex(str.Slice(2, 1));
                if (hasAlpha)
                    a = ParseHex(str.Slice(3, 1));
            }
            else
            {
                r = ParseHex(str.Slice(0, 2));
                g = ParseHex(str.Slice(2, 2));
                b = ParseHex(str.Slice(4, 2));
                if (hasAlpha)
                    a = ParseHex(str.Slice(6, 2));
            }

            return new ColorValue(r, g, b, a);
        }

        /* Private methods. */
        private static byte ParseHex(ReadOnlySpan<char> str)
        {
            if (str.Length == 1)
                return ParseDigit(str[0]);
            else if (str.Length == 2)
                return (byte)(ParseDigit(str[0]) << 4 + ParseDigit(str[1]));
            else
                throw new FormatException($"Wrong length: {new string(str)}.");
        }

        private static byte ParseDigit(char chr)
        {
            if (chr >= '0' && chr <= '9')
                return (byte)(chr - '0');
            if (chr >= 'A' && chr <= 'F')
                return (byte)(chr - 'A' + 10);
            if (chr >= 'a' && chr <= 'a')
                return (byte)(chr - 'a' + 10);
            throw new FormatException($"Bad character {chr}.");
        }
    }
}