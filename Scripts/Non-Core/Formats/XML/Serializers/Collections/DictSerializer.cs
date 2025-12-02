using System.Text;
using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML dictionary serializer.
    /// </summary>
    public class DictSerializer : XmlSerializer<DictNode>
    {
        /* Public properties. */
        public override string Tag => "dict";

        /* Public methods. */
        public override XmlElement ToXml(DictNode node, IXmlSerializerScheme scheme)
        {
            XmlElement element = XmlUtility.Create(Tag);
            foreach (var pair in node.Pairs)
            {
                XmlElement root = XmlUtility.Create("element");

                XmlElement key = scheme.ToXml(pair.Key);
                root.AppendChild(key);

                XmlElement value = scheme.ToXml(pair.Value);
                root.AppendChild(value);

                element.AppendChild(root);
            }
            return element;
        }

        public override DictNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}