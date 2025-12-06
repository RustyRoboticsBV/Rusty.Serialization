using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Numerics;
using System.Xml;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML time serializer.
    /// </summary>
    public class TimeSerializer : XmlSerializer<TimeNode>
    {
        /* Public properties. */
        public override string Tag => "time";

        /* Public methods. */
        public override XmlElement ToXml(TimeNode node, IXmlSerializerScheme scheme)
        {
            XmlElement year = XmlUtility.Pack(node.Value.negative ? node.Value.year.ToString() : (-node.Value.year).ToString(), "Y");
            XmlElement month = XmlUtility.Pack(node.Value.month.ToString(), "M");
            XmlElement day = XmlUtility.Pack(node.Value.day.ToString(), "D");
            XmlElement hour = XmlUtility.Pack(node.Value.hour.ToString(), "h");
            XmlElement minute = XmlUtility.Pack(node.Value.minute.ToString(), "m");
            XmlElement second = XmlUtility.Pack(node.Value.second.ToString(), "s");
            XmlElement millisecond = XmlUtility.Pack(node.Value.millisecond.ToString(), "f");
            return XmlUtility.Pack(new XmlElement[] { year, month, day, hour, minute, second, millisecond }, Tag);
        }

        public override TimeNode FromXml(XmlElement element, IXmlSerializerScheme scheme)
        {
            // Enforce name.
            if (element.Name != Tag)
                throw new ArgumentException("index wasn't " + Tag);

            // Parse element.
            BigInteger year = 0;
            BigInteger month = 0;
            BigInteger day = 0;
            BigInteger hour = 0;
            BigInteger minute = 0;
            BigInteger second = 0;
            BigInteger millisecond = 0;
            foreach (XmlNode child in element)
            {
                if (child is XmlElement childElement)
                {
                    switch (childElement.Name)
                    {
                        case "Y":
                            year = BigInteger.Parse(childElement.InnerText);
                            break;
                        case "M":
                            month = BigInteger.Parse(childElement.InnerText);
                            break;
                        case "D":
                            day = BigInteger.Parse(childElement.InnerText);
                            break;
                        case "h":
                            hour = BigInteger.Parse(childElement.InnerText);
                            break;
                        case "m":
                            minute = BigInteger.Parse(childElement.InnerText);
                            break;
                        case "s":
                            second = BigInteger.Parse(childElement.InnerText);
                            break;
                        case "f":
                            millisecond = BigInteger.Parse(childElement.InnerText);
                            break;
                    }
                }
            }

            return new(new(year, month, day, hour, minute, second, millisecond));
        }
    }
}