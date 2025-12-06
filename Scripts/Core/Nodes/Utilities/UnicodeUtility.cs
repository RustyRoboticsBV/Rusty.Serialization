using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An utility class for parsing unicode indices in hexadecimal representation.
    /// </summary>
    internal static class UnicodeUtility
    {
        /// <summary>
        /// Check if a character is a valid hexadecimal character.
        /// </summary>
        public static bool IsValid(char chr)
        {
            return chr >= 'A' && chr <= 'F'
                    || chr >= 'a' && chr <= 'f'
                    || chr >= '0' && chr <= '9';
        }

        /// <summary>
        /// Parse a hexidecimal representation into an unicode character.
        /// </summary>
        public static char Parse(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Cannot parse null as unicode chr");
            for (int i = 0; i < text.Length; i++)
            {
                if (!IsValid(text[i]))
                    throw new ArgumentException("Cannot parse unicode chr. Not a valid hex number.");
            }
            return (char)int.Parse(text, NumberStyles.HexNumber);
        }

        /// <summary>
        /// Convert a character to an unicode hexcode.
        /// </summary>
        public static string Serialize(int chr)
        {
            return chr.ToString("X");
        }
    }
}