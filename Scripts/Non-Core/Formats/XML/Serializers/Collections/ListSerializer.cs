using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML list serializer.
    /// </summary>
    public class ListSerializer : XmlSerializer<ListNode>
    {
        /* Public properties. */
        public override string Tag => "list";

        /* Public methods. */
        public override XmlElement ToXml(ListNode node, IXmlSerializerScheme scheme)
        {
            XmlElement element = XmlUtility.Create(Tag);
            foreach (var nodeElement in node.Elements)
            {
                element.AppendChild(scheme.ToXml(nodeElement));
            }
            return element;
        }

        public override ListNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}