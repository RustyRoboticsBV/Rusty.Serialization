using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML null serializer.
    /// </summary>
    public class NullSerializer : XmlSerializer<NullNode>
    {
        /* Public properties. */
        public override string Tag => "null";

        /* Public methods. */
        public override XmlElement ToXml(NullNode node, IXmlSerializerScheme scheme)
        {
            return XmlUtility.Create(Tag);
        }

        public override NullNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}