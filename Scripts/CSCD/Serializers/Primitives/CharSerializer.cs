using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD char serializer.
    /// </summary>
    public class CharSerializer : TextSerializer<CharNode>
    {
        /* Protected properties. */
        protected override string StartDelimiter => "\'";
        protected override string EndDelimiter => "\'";
        protected override AllowedCharacterRange[] AllowedCharacters => new AllowedCharacterRange[]
        {
            new AllowedCharacterRange(' ', '~'),
            new AllowedCharacterRange(0xA1, 0xAC),
            new AllowedCharacterRange(0xAE, 0xFF)
        };
        protected override EscapeCharacter[] EscapeCharacters => new EscapeCharacter[]
        {
            ('\\', "\\\\"),
            ('\t', "\\t"),
            ('\n', "\\n")
        };

        /* Protected methods. */
        protected override string EscapeUnicode(string text, int index, int length)
        {
            int codePoint = GetCodePointAt(text, index, length);
            return "\\" + codePoint.ToString("X", CultureInfo.InvariantCulture) + "\\";
        }

        protected override int GetUnicodeLength(string text, int index)
        {
            if (text[index] != '\\')
                return -1;

            for (int i = index + 1; i < text.Length; i++)
            {
                if (text[i] == '\\')
                    return i - index + 1;
            }

            throw new ArgumentException($"Unclosed Unicode sequence in {text}.");
        }

        protected override string ParseUnicode(string text, int index, int length)
        {
            string hex = text.Substring(index + 1, length - 2);
            int codePoint = Convert.ToInt32(hex, 16);

            if (codePoint <= 0xFFFF)
                return ((char)codePoint).ToString();
            else
            {
                codePoint -= 0x10000;

                char high = (char)((codePoint >> 10) + 0xD800);
                char low = (char)((codePoint & 0x3FF) + 0xDC00);

                return string.Concat(high, low);
            }
        }

        /* Private methods. */
        private static bool IsSingleCharacter(string str)
        {
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
    }
}