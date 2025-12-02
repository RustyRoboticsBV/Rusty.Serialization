using System;
using System.Text;

namespace Rusty.Serialization.Serializers.XML
{
    public static class XmlUtility
    {
        public static string Pack(string str, string xmlTag, params (string, string)[] attributePairs)
        {
            StringBuilder attributes = new();
            foreach (var attr in attributePairs)
            {
                attributes.Append(" \"" + attr.Item1 + "\"=\"" + attr.Item2 + '"');
            }

            str = str?.Trim();
            if (str == "")
                return $"<{xmlTag}{attributes}/>";
            /*System.Xml.XmlDocument doc = new();
            System.Xml.XmlElement element = doc.CreateElement("char")
            element.InnerText = str;
            return doc.OuterXml;*/
            return $"<{xmlTag}{attributes}>{str}</{xmlTag}>";
        }

        public static string Unpack(string str, string xmlTag)
        {
            str = str?.Trim();
            if (str == $"<{xmlTag}/>")
                return "";
            if (!str.StartsWith($"<{xmlTag}>") || !str.EndsWith($"</{xmlTag}>"))
                throw new ArgumentException($"Missing or malformed {xmlTag} tag.");
            return str.Substring(xmlTag.Length + 2, str.Length - (xmlTag.Length * 2 + 5));
        }
    }
}