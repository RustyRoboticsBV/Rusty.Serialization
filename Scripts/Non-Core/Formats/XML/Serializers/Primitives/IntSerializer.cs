using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Numerics;
using System.Xml;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML int serializer.
    /// </summary>
    public class IntSerializer : XmlSerializer<IntNode>
    {
        /* Public properties. */
        public override string Tag => "int";

        /* Public methods. */
        public override XmlElement ToXml(IntNode node, IXmlSerializerScheme scheme)
        {
            return XmlUtility.Pack(node.Value.ToString(), Tag);
        }

        public override IntNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            // Enforce name.
            if (element.Name != Tag)
                throw new ArgumentException("Name wasn't " + Tag);

            // Parse element.
            return new(decimal.Parse(element.InnerText));
        }
    }
}