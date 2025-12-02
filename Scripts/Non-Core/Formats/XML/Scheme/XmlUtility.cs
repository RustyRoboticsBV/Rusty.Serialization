using System.Text;
using System.Xml;

namespace Rusty.Serialization.Serializers.XML
{
    public static class XmlUtility
    {
        /* Fields. */
        /// <summary>
        /// Used for creating elements.
        /// </summary>
        private static XmlDocument doc = new();

        /* Public methods. */
        /// <summary>
        /// Create an XML element with some name and attributes.
        /// </summary>
        public static XmlElement Create(string name, params (string, string)[] attributePairs)
        {
            doc.RemoveAll();
            XmlElement element = doc.CreateElement(name);
            foreach (var attribute in attributePairs)
            {
                element.SetAttribute(attribute.Item1, attribute.Item2);
            }
            return element;
        }

        /// <summary>
        /// Convert a string of text into an XML element.
        /// </summary>
        public static XmlElement Pack(string innerText, string name, params (string, string)[] attributePairs)
        {
            XmlElement element = Create(name);
            element.InnerText = innerText.ToString();
            return element;
        }

        /// <summary>
        /// Pack an XML element inside of another element.
        /// </summary>
        public static XmlElement Pack(XmlElement innerElement, string name, params (string, string)[] attributePairs)
        {
            XmlElement element = Create(name, attributePairs);
            element.AppendChild(innerElement);
            return element;
        }

        /// <summary>
        /// Pack multiple XML elements inside of another element.
        /// </summary>
        public static XmlElement Pack(XmlElement[] innerElement, string name, params (string, string)[] attributePairs)
        {
            XmlElement element = Create(name, attributePairs);
            foreach (var inner in innerElement)
            {
                element.AppendChild(inner);
            }
            return element;
        }

        /// <summary>
        /// Parse a string of XML into a document object.
        /// </summary>
        public static XmlElement Parse(string xml)
        {
            XmlDocument doc = new();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc)
            {
                if (node is XmlElement element)
                    return element;
            }
            return null;
        }

        /// <summary>
        /// Create an XML document from an XML element.
        /// </summary>
        public static XmlDocument Doc(XmlElement element)
        {
            doc.RemoveAll();
            XmlNode node = doc.ImportNode(element, true);
            doc.AppendChild(node);
            return doc;
        }

        /// <summary>
        /// Print an XML document.
        /// </summary>
        public static string Print(XmlDocument doc)
        {
            var settings = new XmlWriterSettings
            {
                Indent = false,
                IndentChars = "",
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace
            };

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Print an XML element.
        /// </summary>
        public static string Print(XmlElement element)
        {
            return Print(Doc(element));
        }

        /// <summary>
        /// Pretty print an XML document.
        /// </summary>
        public static string PrettyPrint(XmlDocument doc)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n",
                NewLineHandling = NewLineHandling.Replace
            };

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Pretty print an XML element.
        /// </summary>
        public static string PrettyPrint(XmlElement element)
        {
            return PrettyPrint(Doc(element));
        }
    }
}