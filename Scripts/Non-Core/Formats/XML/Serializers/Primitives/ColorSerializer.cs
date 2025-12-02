using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Xml;

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
            // Enforce name.
            if (element.Name != Tag)
                throw new ArgumentException("Name wasn't " + Tag);

            // Parse element.
            byte r = 0;
            byte g = 0;
            byte b = 0;
            byte a = 1;

            foreach (XmlNode child in element)
            {
                if (child is XmlElement childElement)
                {
                    switch (childElement.Name)
                    {
                        case "r":
                            r = byte.Parse(childElement.InnerText);
                            break;
                        case "g":
                            g = byte.Parse(childElement.InnerText);
                            break;
                        case "b":
                            b = byte.Parse(childElement.InnerText);
                            break;
                        case "a":
                            a = byte.Parse(childElement.InnerText);
                            break;
                    }
                }
            }

            return new(r, g, b, a);
        }
    }
}