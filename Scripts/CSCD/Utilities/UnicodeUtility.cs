using System;
using System.Globalization;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// An utility class for parsing unicode indices in hexadecimal representation.
    /// </summary>
    internal static class UnicodeUtility
    {
        /// <summary>
        /// Check if a character is a valid hexadecimal character.
        /// </summary>
        public static bool IsHexCharacter(char chr)
        {
            return chr >= 'A' && chr <= 'F'
                    || chr >= 'a' && chr <= 'f'
                    || chr >= '0' && chr <= '9';
        }

        /// <summary>
        /// Parse a hexidecimal string into an Unicode code point.
        /// </summary>
        public static int HexToCodePoint(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException($"'{str}' is not a valid hex number.");
            for (int i = 0; i < str.Length; i++)
            {
                if (!IsHexCharacter(str[i]))
                    throw new ArgumentException($"'{str}' is not a valid hex number.");
            }
            return int.Parse(str, NumberStyles.HexNumber);
        }

        /// <summary>
        /// Convert a Unicode code-point into a hex string.
        /// </summary>
        public static string CodePointToHex(int codePoint)
        {
            return codePoint.ToString("X", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if a string represents a single Unicode character.
        /// </summary>
        public static bool IsSingleUnicodeCharacter(string str)
        {
            // Empty string.
            if (string.IsNullOrEmpty(str))
                return false;

            // Single BMP character.
            if (str.Length == 1)
                return true;

            // Surrogate pair.
            else if (str.Length == 2)
                return char.IsHighSurrogate(str[0]) && char.IsLowSurrogate(str[1]);

            // Any other length cannot be a single Unicode character.
            return false;
        }

        /// <summary>
        /// Get the code-point of a valid unicode single-character string.
        /// </summary>
        public static int UnicodeToCodePoint(string str)
        {
            // Validate string.
            if (!IsSingleUnicodeCharacter(str))
                throw new ArgumentException($"'{str}' is not a valid Unicode character.");

            // Single BMP character.
            if (str.Length == 1)
                return str[0];

            // Surrogate pair.
            else
            {
                char high = str[0];
                char low = str[1];
                return 0x10000 + ((high - 0xD800) << 10) + (low - 0xDC00);
            }
        }
        
        /// <summary>
        /// Get a single-character Unicode string from a code-point.
        /// </summary>
        public static string CodePointToUnicode(int codePoint)
        {
            if (codePoint < 0 || codePoint > 0x10FFFF)
                throw new ArgumentOutOfRangeException(nameof(codePoint), $"Code point {codePoint} is outside the valid Unicode range (0x0 - 0x10FFFF).");

            // BMP character.
            if (codePoint <= 0xFFFF)
            {
                return new string((char)codePoint, 1);
            }

            // Surrogate pair.
            else
            {
                codePoint -= 0x10000;
                char high = (char)((codePoint >> 10) + 0xD800);
                char low = (char)((codePoint & 0x3FF) + 0xDC00);

                Span<char> buffer = stackalloc char[2];
                buffer[0] = high;
                buffer[1] = low;
                return new string(buffer);
            }
        }

        /// <summary>
        /// Convert a single-character Unicode string to a hexadecimal string.
        /// </summary>
        public static string UnicodeToHex(string str)
        {
            return CodePointToHex(UnicodeToCodePoint(str));
        }

        /// <summary>
        /// Convert a hexadecimal string to a single-character Unicode string.
        /// </summary>
        public static string HextoUnicode(string str)
        {
            return CodePointToUnicode(HexToCodePoint(str));
        }
    }
}