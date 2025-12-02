using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System.Xml;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML serializer.
    /// </summary>
    public interface IXmlSerializer<T> : ISerializer<T>
        where T : INode
    {
        /* Public properties. */
        /// <summary>
        /// The XML tag associated with this serializer.
        /// </summary>
        public string Tag { get; }

        /* Public methods. */
        /// <summary>
        /// Convert a serializer node to an XML element.
        /// </summary>
        public XmlElement ToXml(T node, IXmlSerializerScheme scheme);

        /// <summary>
        /// Convert an XML element to a serializer node.
        /// </summary>
        public abstract T FromXml(XmlElement element, IXmlSerializerScheme scheme);
    }
}