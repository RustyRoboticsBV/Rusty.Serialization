using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD string serializer.
    /// </summary>
    public class StringSerializer : TextSerializer<StringNode>
    {
        /* Protected properties. */
        protected override string StartDelimiter => "\"";
        protected override string EndDelimiter => "\"";
        protected override AllowedCharacterRange[] AllowedCharacters => new AllowedCharacterRange[]
        {
            new AllowedCharacterRange(' ', '~'),
            new AllowedCharacterRange(0xA1, 0xAC),
            new AllowedCharacterRange(0xAE, 0xFF)
        };
        protected override EscapeCharacter[] EscapeCharacters => new EscapeCharacter[]
        {
            new EscapeCharacter('\\', "\\\\"),
            new EscapeCharacter('\t', "\\t"),
            new EscapeCharacter('\n', "\\n")
        };

        /* Protected methods. */
        protected override string EscapeUnicode(string text, int index, int length)
        {
            // Get code point str.
            int codePoint;
            if (length == 2)
            {
                char high = text[index];
                char low = text[index + 1];

                codePoint = 0x10000 +
                    ((high - 0xD800) << 10) +
                    (low - 0xDC00);
            }
            else if (length == 1)
                codePoint = text[index];
            else
                throw new ArgumentException($"Bad length {length}.");

            // Wrap in correct escape sequence.
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
    }
}