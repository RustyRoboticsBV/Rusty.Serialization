using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

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
            throw new System.NotImplementedException();
        }
    }
}