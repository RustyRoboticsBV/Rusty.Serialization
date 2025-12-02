using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
            List<KeyValuePair<INode, INode>> elements = new();
            foreach (var member in element.ChildNodes)
            {
                if (member is XmlElement childElement && childElement.Name == "element" && childElement.ChildNodes.Count == 2
                     && childElement.ChildNodes[0] is XmlElement key && childElement.ChildNodes[1] is XmlElement value)
                {
                    INode keyNode = scheme.FromXml(key);
                    INode valueNode = scheme.FromXml(value);
                    elements.Add(new(keyNode, valueNode));
                }
            }
            return new(elements.ToArray());
        }
    }
}