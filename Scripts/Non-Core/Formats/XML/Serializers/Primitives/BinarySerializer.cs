using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

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
            throw new System.NotImplementedException();
        }
    }
}