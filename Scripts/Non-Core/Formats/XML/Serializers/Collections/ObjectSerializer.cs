using System.Collections.Generic;
using System.Xml;
using Rusty.Serialization.Core.Nodes;

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
                value.SetAttribute("index", member.Key);
                element.AppendChild(value);
            }
            return element;
        }

        public override ObjectNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            List<KeyValuePair<string, INode>> elements = new();
            foreach (var member in element.ChildNodes)
            {
                if (member is XmlElement childElement)
                {
                    string key = childElement.GetAttribute("index");
                    INode value = scheme.FromXml(childElement);
                    elements.Add(new(key, value));
                }
            }
            return new(elements.ToArray());
        }
    }
}