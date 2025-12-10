using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A utility for parsing and converting to hex strings.
    /// </summary>
    internal static class HexUtility
    {
        /* Private fields. */
        private static readonly char[] HexLookup = "0123456789ABCDEF".ToCharArray();

        /* Public methods. */
        /// <summary>
        /// Converts an int to a hex string.
        /// </summary>
        public static string ToHexString(int value)
        {
            uint u = unchecked((uint)value);

            // Special case zero
            if (u == 0)
                return "0";

            // Count needed hex digits
            int digitCount = 0;
            uint temp = u;
            while (temp != 0)
            {
                digitCount++;
                temp >>= 4;
            }

            char[] buffer = new char[digitCount];

            for (int i = digitCount - 1; i >= 0; i--)
            {
                buffer[i] = HexLookup[u & 0xF];
                u >>= 4;
            }

            return new string(buffer);
        }

        /// <summary>
        /// Converts a byte[] to a hex string.
        /// </summary>
        public static string ToHexString(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            int len = value.Length;
            if (len == 0)
                return string.Empty;

            char[] buffer = new char[len * 2];

            for (int i = 0; i < len; i++)
            {
                byte b = value[i];
                int idx = i * 2;

                buffer[idx] = HexLookup[b >> 4];
                buffer[idx + 1] = HexLookup[b & 0xF];
            }

            return new string(buffer);
        }


        /// <summary>
        /// Parses a hex string as a number.
        /// </summary>
        public static int FromHexString(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException(nameof(hex));

            if (hex.Length == 0)
                throw new FormatException("Hex string cannot be empty.");

            if (hex.Length > 8)
                throw new OverflowException("Hex string too long for a 32-bit integer.");

            uint result = 0;

            for (int i = 0; i < hex.Length; i++)
            {
                result <<= 4;

                char c = hex[i];
                if (c >= '0' && c <= '9')
                    result |= (uint)(c - '0');
                else if (c >= 'A' && c <= 'F')
                    result |= (uint)(c - 'A' + 10);
                else if (c >= 'a' && c <= 'f')
                    result |= (uint)(c - 'a' + 10);
                else
                    throw new FormatException($"Invalid hex character '{c}' at position {i}.");
            }

            return unchecked((int)result);
        }

        /// <summary>
        /// Parses a hex string as a byte[].
        /// </summary>
        public static byte[] BytesFromHexString(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException(nameof(hex));

            int len = hex.Length;

            if (len == 0)
                return Array.Empty<byte>();

            if (len % 2 != 0)
                throw new FormatException("Hex string length must be even.");

            byte[] result = new byte[len / 2];

            for (int i = 0; i < result.Length; i++)
            {
                int hi = FromHexChar(hex[i * 2]);
                int lo = FromHexChar(hex[i * 2 + 1]);
                result[i] = (byte)((hi << 4) | lo);
            }

            return result;
        }

        /* Private methods. */
        private static int FromHexChar(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';

            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;

            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            throw new FormatException($"Invalid hex character: '{c}'");
        }
    }
}