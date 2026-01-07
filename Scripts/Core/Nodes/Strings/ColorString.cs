using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string that represents a color in hex form:
    /// RGB, RGBA, RRGGBB or RRGGBBAA
    /// </summary>
    public readonly struct ColorString : IEquatable<ColorString>
    {
        /* Fields */
        private readonly string value;

        /* Public properties */
        public bool HasAlpha => value.Length == 4 || value.Length == 8;

        public byte R => ParseComponent(0);
        public byte G => ParseComponent(HasShortForm ? 1 : 2);
        public byte B => ParseComponent(HasShortForm ? 2 : 4);
        public byte A => HasAlpha ? ParseComponent(HasShortForm ? 3 : 6) : (byte)255;

        public bool HasShortForm => value.Length == 3 || value.Length == 4;

        /* Constructors */
        private ColorString(string value)
        {
            this.value = value;
        }

        public ColorString(byte r, byte g, byte b, byte a)
        {
            // Convert to 2-digit hex.
            string rstr = r.ToString("X2");
            string gstr = g.ToString("X2");
            string bstr = b.ToString("X2");
            string astr = a.ToString("X2");

            // Check if short form is possible.
            bool shortRgb =
                rstr[0] == rstr[1] &&
                gstr[0] == gstr[1] &&
                bstr[0] == bstr[1];

            bool shortAlpha = shortRgb && astr[0] == astr[1];

            // Omit alpha if fully opaque.
            if (a == 255)
            {
                value = shortRgb
                    ? $"{rstr[0]}{gstr[0]}{bstr[0]}"
                    : $"{rstr}{gstr}{bstr}";
            }
            else
            {
                value = shortAlpha
                    ? $"{rstr[0]}{gstr[0]}{bstr[0]}{astr[0]}"
                    : $"{rstr}{gstr}{bstr}{astr}";
            }
        }

        /* Conversion operators */
        public static implicit operator ColorString(string text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            string trimmed = text.Trim().ToUpperInvariant();

            if (trimmed.Length == 0)
                throw new FormatException("Empty string is not a valid color.");

            if (trimmed.Length != 3 &&
                trimmed.Length != 4 &&
                trimmed.Length != 6 &&
                trimmed.Length != 8)
            {
                throw new FormatException(
                    "Invalid color format. Use RGB, RGBA, RRGGBB or RRGGBBAA.");
            }

            for (int i = 0; i < trimmed.Length; i++)
            {
                char c = trimmed[i];
                if (!IsHex(c))
                    throw new FormatException("Color contains non-hexadecimal characters.");
            }

            return new ColorString(trimmed);
        }

        public static implicit operator ColorString((byte, byte, byte, byte) value)
        {
            return new ColorString(value.Item1, value.Item2, value.Item3, value.Item4);
        }

        public static implicit operator string(ColorString value) => value.value;

        /* Public methods */
        public override string ToString() => value;
        public override int GetHashCode() => value?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => obj is ColorString other && Equals(other);
        public bool Equals(ColorString other) => value == other.value;

        /* Private methods. */
        private static bool IsHex(char c)
            => (c >= '0' && c <= '9')
            || (c >= 'A' && c <= 'F');

        private byte ParseComponent(int index)
        {
            if (value == null)
                return 0;

            if (HasShortForm)
            {
                char c = value[index];
                string hex = new string(c, 2);
                return byte.Parse(hex, NumberStyles.HexNumber);
            }

            return byte.Parse(
                value.Substring(index, 2),
                NumberStyles.HexNumber);
        }
    }
}