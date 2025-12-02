using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Xml;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML char serializer.
    /// </summary>
    public class CharSerializer : XmlSerializer<CharNode>
    {
        /* Public properties. */
        public override string Tag => "char";

        /* Public methods. */
        public override XmlElement ToXml(CharNode node, IXmlSerializerScheme scheme)
        {
            string str;
            if (node.Value <= char.MaxValue)
                str = ((char)node.Value).ToString();
            else
                str = $"\\[{HexUtility.ToHexString(node.Value)}]";
            return XmlUtility.Pack(str, Tag);
        }

        public override CharNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            // Enforce name.
            if (element.Name != Tag)
                throw new ArgumentException("Name wasn't " + Tag);

            // Try to parse string.
            string contents = element.InnerText;

            // Parse contents.
            int chr;
            if (contents.StartsWith("\\[") && contents.EndsWith("]"))
                chr = HexUtility.FromHexString(contents.Substring(2, contents.Length - 3));
            else if (contents.Length > 1)
                throw new Exception("Too many characters.");
            else
                chr = contents[0];

            // Return node.
            return new(chr);
        }
    }
}