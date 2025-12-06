using System;
using System.Xml;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML string serializer.
    /// </summary>
    public class StringSerializer : XmlSerializer<StringNode>
    {
        /* Public properties. */
        public override string Tag => "str";

        /* Public methods. */
        public override XmlElement ToXml(StringNode node, IXmlSerializerScheme scheme)
        {
            return XmlUtility.Pack(node.Value, Tag);
        }

        public override StringNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            // Enforce name.
            if (element.Name != Tag)
                throw new ArgumentException("index wasn't " + Tag);

            // Return node.
            return new(element.InnerText);
        }
    }
}