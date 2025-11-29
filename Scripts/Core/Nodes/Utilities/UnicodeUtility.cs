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
        /// Parse a hexidecimal representation into an unicode character.
        /// </summary>
        public static char Parse(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Cannot parse null as unicode character");
            for (int i = 0; i < text.Length; i++)
            {
                if (
                    !(text[i] >= 'A' && text[i] <= 'F'
                    || text[i] >= 'a' && text[i] <= 'f'
                    || text[i] >= '0' && text[i] <= '9')
                )
                {
                    throw new ArgumentException("Cannot parse unicode character. Not a valid hex number.");
                }
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