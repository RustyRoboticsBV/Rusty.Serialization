using System;
using System.Xml;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML binary serializer.
    /// </summary>
    public class BinarySerializer : XmlSerializer<BinaryNode>
    {
        /* Public properties. */
        public override string Tag => "bin";

        /* Public methods. */
        public override XmlElement ToXml(BinaryNode node, IXmlSerializerScheme scheme)
        {
            return XmlUtility.Pack(HexUtility.ToHexString(node.Value), Tag);
        }

        public override BinaryNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            // Enforce name.
            if (element.Name != Tag)
                throw new ArgumentException("Name wasn't " + Tag);

            // Parse binary string.
            byte[] binary = HexUtility.BytesFromHexString(element.InnerText);

            // Return node.
            return new(binary);
        }
    }
}