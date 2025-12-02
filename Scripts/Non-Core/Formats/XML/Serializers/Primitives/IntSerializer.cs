using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

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
            throw new System.NotImplementedException();
        }
    }
}