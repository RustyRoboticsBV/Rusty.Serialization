using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML bool serializer.
    /// </summary>
    public class BoolSerializer : XmlSerializer<BoolNode>
    {
        /* Public properties. */
        public override string Tag => "bool";

        /* Public methods. */
        public override XmlElement ToXml(BoolNode node, IXmlSerializerScheme scheme)
        {
            return XmlUtility.Pack(node.Value ? "true" : "false", Tag);
        }

        public override BoolNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}