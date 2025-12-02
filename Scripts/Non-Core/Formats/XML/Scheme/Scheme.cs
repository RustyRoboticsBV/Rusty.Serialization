using System;
using System.Xml;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// A CSCD serialization scheme.
    /// </summary>
    public class Scheme : IXmlSerializerScheme
    {
        /* Public properties. */
        public bool PrettyPrint { get; set; }
        public string Tab { get; set; } = "  ";

        /* Private properties. */
        private NullSerializer Null { get; } = new();
        private BoolSerializer Bool { get; } = new();
        private IntSerializer Int { get; } = new();
        private RealSerializer Real { get;  } = new();
        private CharSerializer Char { get; } = new();
        private StringSerializer String { get; } = new();
        private ColorSerializer Color { get; } = new();
        private TimeSerializer Time { get; } = new();
        private BinarySerializer Binary { get; } = new();
        private ListSerializer List { get; } = new();
        private DictSerializer Dict { get; } = new();
        private ObjectSerializer Object { get; } = new();
        private TypeSerializer Type { get; } = new();

        /* Public methods. */
        public string Serialize(INode node)
        {
            switch (node)
            {
                case NullNode n:
                    return Null.Serialize(n, this);
                case BoolNode b:
                    return Bool.Serialize(b, this);
                case IntNode i:
                    return Int.Serialize(i, this);
                case RealNode r:
                    return Real.Serialize(r, this);
                case CharNode c:
                    return Char.Serialize(c, this);
                case StringNode s:
                    return String.Serialize(s, this);
                case ColorNode col:
                    return Color.Serialize(col, this);
                case TimeNode t:
                    return Time.Serialize(t, this);
                case BinaryNode bin:
                    return Binary.Serialize(bin, this);
                case ListNode l:
                    return List.Serialize(l, this);
                case DictNode d:
                    return Dict.Serialize(d, this);
                case ObjectNode o:
                    return Object.Serialize(o, this);
                case TypeNode ty:
                    return Type.Serialize(ty, this);
                default:
                    throw new ArgumentException($"Unknown node type '{node.GetType()}'.");
            }
        }

        public INode Parse(string serialized)
        {
            return FromXml(XmlUtility.Parse(serialized));
        }

        public XmlElement ToXml(INode node)
        {
            switch (node)
            {
                case NullNode n:
                    return Null.ToXml(n, this);
                case BoolNode b:
                    return Bool.ToXml(b, this);
                case IntNode i:
                    return Int.ToXml(i, this);
                case RealNode r:
                    return Real.ToXml(r, this);
                case CharNode c:
                    return Char.ToXml(c, this);
                case StringNode s:
                    return String.ToXml(s, this);
                case ColorNode col:
                    return Color.ToXml(col, this);
                case TimeNode t:
                    return Time.ToXml(t, this);
                case BinaryNode bin:
                    return Binary.ToXml(bin, this);
                case ListNode l:
                    return List.ToXml(l, this);
                case DictNode d:
                    return Dict.ToXml(d, this);
                case ObjectNode o:
                    return Object.ToXml(o, this);
                case TypeNode ty:
                    return Type.ToXml(ty, this);
                default:
                    throw new ArgumentException($"Unknown node type '{node.GetType()}'.");
            }
        }

        public INode FromXml(XmlElement element)
        {
            if (element.Name == Null.Tag)
                return Null.FromXml(element, this);
            else if (element.Name == Bool.Tag)
                return Bool.FromXml(element, this);
            else if (element.Name == Int.Tag)
                return Int.FromXml(element, this);
            else if (element.Name == Real.Tag)
                return Real.FromXml(element, this);
            else if (element.Name == Char.Tag)
                return Char.FromXml(element, this);
            else if (element.Name == String.Tag)
                return String.FromXml(element, this);
            else if (element.Name == Color.Tag)
                return Color.FromXml(element, this);
            else if (element.Name == Time.Tag)
                return Time.FromXml(element, this);
            else if (element.Name == Binary.Tag)
                return Binary.FromXml(element, this);
            else if (element.Name == List.Tag)
                return List.FromXml(element, this);
            else if (element.Name == Dict.Tag)
                return Dict.FromXml(element, this);
            else if (element.Name == Object.Tag)
                return Object.FromXml(element, this);
            else
                throw new Exception($"Invalid XML element type {element.Name}.");
        }
    }
}
