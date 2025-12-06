using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Xml;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML bool serializer.
    /// </summary>
    public class BoolSerializer : XmlSerializer<BoolNode>
    {
        /* Public properties. */
        public override string Tag => "bool";

        /* Public methods. */
        public override XmlElement ToXml(BoolNode node, IXmlSerializerScheme scheme)
        {
            return XmlUtility.Pack(node.Value ? "true" : "false", Tag);
        }

        public override BoolNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            // Enforce name.
            if (element.Name != Tag)
                throw new ArgumentException("index wasn't " + Tag);

            // Enforce 'true' or 'false'.
            if (element.InnerText != "true" && element.InnerText != "false")
                throw new ArgumentException("Bad bool literal.");

            // Return node.
            return new(element.InnerText == "true");
        }
    }
}