using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

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
            throw new System.NotImplementedException();
        }
    }
}