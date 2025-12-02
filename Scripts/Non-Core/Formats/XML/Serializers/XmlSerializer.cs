using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// A base class for XML serializers.
    /// </summary>
    public abstract class XmlSerializer<T> : Serializer<T>, IXmlSerializer<T>
        where T : INode
    {
        /* Public properties. */
        public abstract string Tag { get; }

        /* Public methods. */
        public sealed override string Serialize(T node, ISerializerScheme scheme)
        {
            XmlElement element = ToXml(node, scheme as IXmlSerializerScheme);
            if (scheme.PrettyPrint)
                return XmlUtility.PrettyPrint(element);
            else
                return XmlUtility.Print(element);
        }

        public abstract XmlElement ToXml(T node, IXmlSerializerScheme scheme);

        public sealed override T Parse(string text, ISerializerScheme scheme)
        {
            XmlElement element = XmlUtility.Parse(text);
            return FromXml(element, scheme as IXmlSerializerScheme);
        }

        public abstract T FromXml(XmlElement element, IXmlSerializerScheme scheme);
    }
}