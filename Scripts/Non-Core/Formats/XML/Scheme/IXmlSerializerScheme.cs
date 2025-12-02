using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML serialization scheme.
    /// </summary>
    public interface IXmlSerializerScheme : ISerializerScheme
    {
        /* Public methods. */
        /// <summary>
        /// Convert an INode to an XML document.
        /// </summary>
        public XmlElement ToXml(INode node);
        /// <summary>
        /// Convert an XML document to an INode.
        /// </summary>
        public INode FromXml(XmlElement doc);
    }
}