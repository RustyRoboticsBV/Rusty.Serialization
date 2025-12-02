using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML type serializer.
    /// </summary>
    public class TypeSerializer : XmlSerializer<TypeNode>
    {
        /* Public properties. */
        public override string Tag => "type";

        /* Public methods. */
        public override XmlElement ToXml(TypeNode node, IXmlSerializerScheme scheme)
        {
            XmlElement element = scheme.ToXml(node.Value);
            element.SetAttribute("type", node.Name);
            return element;
        }

        public override TypeNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}