using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML char serializer.
    /// </summary>
    public class CharSerializer : XmlSerializer<CharNode>
    {
        /* Public properties. */
        public override string Tag => "char";

        /* Public methods. */
        public override XmlElement ToXml(CharNode node, IXmlSerializerScheme scheme)
        {
            string str;
            if (node.Value > char.MaxValue)
                str = ((char)node.Value).ToString();
            else
                str = char.ConvertFromUtf32(node.Value);
            return XmlUtility.Pack(str, Tag);
        }

        public override CharNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            throw new System.NotImplementedException();
        }
    }
}