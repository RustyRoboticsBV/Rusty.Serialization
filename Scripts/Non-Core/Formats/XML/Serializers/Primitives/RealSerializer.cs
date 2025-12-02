using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML real serializer.
    /// </summary>
    public class RealSerializer : XmlSerializer<RealNode>
    {
        /* Public properties. */
        public override string Tag => "real";

        /* Public methods. */
        public override XmlElement ToXml(RealNode node, IXmlSerializerScheme scheme)
        {
            return XmlUtility.Pack(node.Value.ToString(), Tag);
        }

        public override RealNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}