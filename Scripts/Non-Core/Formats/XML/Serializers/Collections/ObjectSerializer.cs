using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML object serializer.
    /// </summary>
    public class ObjectSerializer : XmlSerializer<ObjectNode>
    {
        /* Public properties. */
        public override string Tag => "obj";

        /* Public methods. */
        public override XmlElement ToXml(ObjectNode node, IXmlSerializerScheme scheme)
        {
            XmlElement element = XmlUtility.Create(Tag);
            foreach (var member in node.Members)
            {
                XmlElement value = scheme.ToXml(member.Value);
                value.SetAttribute("name", member.Key);
                element.AppendChild(value);
            }
            return element;
        }

        public override ObjectNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}