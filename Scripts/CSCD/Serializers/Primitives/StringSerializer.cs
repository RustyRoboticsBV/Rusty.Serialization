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
            ('\'', "\\\'"),
            ('\"', "\\\""),
            ('\\', "\\\\"),
            ('\t', "\\t"),
            ('\n', "\\n")
        };

        /* Protected methods. */
        protected override string EscapeUnicode(string text, int index)
        {
            UnicodePair chr = new UnicodePair(text, index);
            return "\\" + chr.Hex + "\\";
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

            return 0;
        }

        protected override UnicodePair ParseUnicode(string text, int index, int length)
        {
            string hex = text.Substring(index + 1, length - 2);
            int codePoint = Convert.ToInt32(hex, 16);
            return new UnicodePair(codePoint);
        }
    }
}