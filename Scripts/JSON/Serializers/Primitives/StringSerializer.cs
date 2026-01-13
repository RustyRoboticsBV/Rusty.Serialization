using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON string serializer.
    /// </summary>
    public class StringSerializer : TextSerializer<StringNode>
    {
        /* Protected properties. */
        protected override string StartDelimiter => "\"";
        protected override string EndDelimiter => "\"";
        protected override AllowedCharacterRange[] AllowedCharacters => new AllowedCharacterRange[]
        {
            new AllowedCharacterRange(0x20, 0x10FFFF)
        };
        protected override EscapeCharacter[] EscapeCharacters => new EscapeCharacter[]
        {
            ('\"', "\\\""),
            ('\\', "\\\\"),
            ('\b', "\\b"),
            ('\f', "\\f"),
            ('\t', "\\t"),
            ('\n', "\\n"),
            ('\r', "\\r")
        };

        /* Protected methods. */
        protected override string EscapeUnicode(string text, int index, int length)
        {
            return text[index].ToString();
        }// TODO: handle unicode escapes.

        protected override int GetUnicodeLength(string text, int index)
        {
            return 0;
        }// TODO: handle unicode escapes.

        protected override string ParseUnicode(string text, int index, int length)
        {
            return text[index].ToString();
        }// TODO: handle unicode escapes.
    }
}