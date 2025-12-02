using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML color serializer.
    /// </summary>
    public class ColorSerializer : XmlSerializer<ColorNode>
    {
        /* Public properties. */
        public override string Tag => "col";

        /* Public methods. */
        public override XmlElement ToXml(ColorNode node, IXmlSerializerScheme scheme)
        {
            XmlElement r = XmlUtility.Pack(node.R.ToString(), "r");
            XmlElement g = XmlUtility.Pack(node.G.ToString(), "g");
            XmlElement b = XmlUtility.Pack(node.B.ToString(), "b");
            XmlElement a = XmlUtility.Pack(node.A.ToString(), "a");
            return XmlUtility.Pack(new XmlElement[] { r, g, b, a }, Tag);
        }

        public override ColorNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}