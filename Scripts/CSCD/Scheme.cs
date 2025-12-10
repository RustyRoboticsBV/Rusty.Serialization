using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD serialization scheme.
    /// </summary>
    public class Scheme : ISerializerScheme
    {
        /* Public properties. */
        public bool PrettyPrint { get; set; }
        public string Tab { get; set; } = "  ";

        /* Private properties. */
        private NullSerializer Null { get; } = new();
        private RefSerializer Ref { get; } = new();
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
        private IdSerializer Id { get; } = new();

        /* Public methods. */
        public string Serialize(INode node)
        {
            switch (node)
            {
                case NodeTree tree:
                    return Serialize(tree.Root);
                case NullNode n:
                    return Null.Serialize(n, this);
                case RefNode re:
                    return Ref.Serialize(re, this);
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
                case IdNode id:
                    return Id.Serialize(id, this);
                default:
                    throw new ArgumentException($"Unknown node type '{node.GetType()}'.");
            }
        }

        public NodeTree ParseAsTree(string serialized)
        {
            INode node = ParseAsNode(serialized);
            NodeTree tree = new(node);
            return tree;
        }

        public INode ParseAsNode(string serialized)
        {
            if (serialized == null)
                throw new ArgumentException("Cannot parse null strings.");

            serialized = serialized.Trim();
            if (serialized.Length == 0)
                throw new ArgumentException("Cannot parse empty strings.");

            // Metadata.
            if (serialized.StartsWith('('))
                return Type.Parse(serialized, this);
            if (serialized.StartsWith('`'))
                return Id.Parse(serialized, this);

            // Collections.
            if (serialized.StartsWith('[') && serialized.EndsWith(']'))
                return List.Parse(serialized, this);
            if (serialized.StartsWith('{') && serialized.EndsWith('}'))
                return Dict.Parse(serialized, this);
            if (serialized.StartsWith('<') && serialized.EndsWith('>'))
                return Object.Parse(serialized, this);

            // Primitives.
            if (serialized.StartsWith('&'))
                return Ref.Parse(serialized, this);
            if (serialized.StartsWith('n'))
                return Null.Parse(serialized, this);
            if (serialized.StartsWith('t') || serialized.StartsWith("fa"))
                return Bool.Parse(serialized, this);
            if (serialized.StartsWith('\'') && serialized.EndsWith('\''))
                return Char.Parse(serialized, this);
            if (serialized.StartsWith('"') && serialized.EndsWith('"'))
                return String.Parse(serialized, this);
            if (serialized.StartsWith('#'))
                return Color.Parse(serialized, this);
            if (serialized.StartsWith("0x"))
                return Binary.Parse(serialized, this);
            if (serialized.StartsWith('Y') || serialized.StartsWith("-Y")
                || serialized.StartsWith('M') || serialized.StartsWith("-M")
                || serialized.StartsWith('D') || serialized.StartsWith("-D")
                || serialized.StartsWith('h') || serialized.StartsWith("-h")
                || serialized.StartsWith('m') || serialized.StartsWith("-m")
                || serialized.StartsWith('s') || serialized.StartsWith("-s")
                || serialized.StartsWith('f') || serialized.StartsWith("-f"))
            {
                return Time.Parse(serialized, this);
            }

            bool startNumeric = (serialized[0] >= '0' && serialized[0] <= '9') || serialized[0] == '-' || serialized[0] == '.';
            if (startNumeric)
            {
                if (serialized.Contains('.'))
                    return Real.Parse(serialized, this);
                else
                    return Int.Parse(serialized, this);
            }

            // Invalid string.
            throw new ArgumentException($"Invalid string '{serialized}'");
        }
    }
}
