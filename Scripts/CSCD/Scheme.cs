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
        private TypeSerializer Type { get; } = new();
        private IdSerializer Id { get; } = new();
        private NullSerializer Null { get; } = new();
        private BoolSerializer Bool { get; } = new();
        private IntSerializer Int { get; } = new();
        private FloatSerializer Float { get;  } = new();
        private NanSerializer Nan { get; } = new();
        private InfinitySerializer Infinity { get; } = new();
        private CharSerializer Char { get; } = new();
        private StringSerializer String { get; } = new();
        private ColorSerializer Color { get; } = new();
        private TimeSerializer Time { get; } = new();
        private DecimalSerializer Decimal { get; } = new();
        private BytesSerializer Bytes { get; } = new();
        private RefSerializer Ref { get; } = new();
        private ListSerializer List { get; } = new();
        private DictSerializer Dict { get; } = new();
        private ObjectSerializer Object { get; } = new();

        /* Public methods. */
        public string Serialize(NodeTree tree, bool prettyPrint = false)
        {
            return Serialize(tree.Root, prettyPrint);
        }

        public string Serialize(INode node, bool prettyPrint = false)
        {
            PrettyPrint = prettyPrint;
            switch (node)
            {
                case TypeNode type:
                    return Type.Serialize(type, this);
                case IdNode id:
                    return Id.Serialize(id, this);
                case NullNode @null:
                    return Null.Serialize(@null, this);
                case BoolNode @bool:
                    return Bool.Serialize(@bool, this);
                case IntNode @int:
                    return Int.Serialize(@int, this);
                case FloatNode @float:
                    return Float.Serialize(@float, this);
                case NanNode nan:
                    return Nan.Serialize(nan, this);
                case InfinityNode inf:
                    return Infinity.Serialize(inf, this);
                case CharNode chr:
                    return Char.Serialize(chr, this);
                case StringNode str:
                    return String.Serialize(str, this);
                case ColorNode color:
                    return Color.Serialize(color, this);
                case TimeNode time:
                    return Time.Serialize(time, this);
                case DecimalNode cur:
                    return Decimal.Serialize(cur, this);
                case BytesNode bytes:
                    return Bytes.Serialize(bytes, this);
                case RefNode @ref:
                    return Ref.Serialize(@ref, this);
                case ListNode list:
                    return List.Serialize(list, this);
                case DictNode dict:
                    return Dict.Serialize(dict, this);
                case ObjectNode obj:
                    return Object.Serialize(obj, this);
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
            if (serialized.StartsWith("nu"))
                return Null.Parse(serialized, this);
            if (serialized.StartsWith('t') || serialized.StartsWith("fa"))
                return Bool.Parse(serialized, this);
            if (serialized.StartsWith("na"))
                return Nan.Parse(serialized, this);
            if (serialized.StartsWith('i') || serialized.StartsWith("-i"))
                return Infinity.Parse(serialized, this);
            if (serialized.StartsWith('\'') && serialized.EndsWith('\''))
                return Char.Parse(serialized, this);
            if (serialized.StartsWith('"') && serialized.EndsWith('"'))
                return String.Parse(serialized, this);
            if (serialized.StartsWith('#'))
                return Color.Parse(serialized, this);
            if (serialized.StartsWith('$') || serialized.StartsWith("-$"))
                return Decimal.Parse(serialized, this);
            if (serialized.StartsWith("b_"))
                return Bytes.Parse(serialized, this);
            if (serialized.StartsWith('Y') || serialized.StartsWith("-Y")
                || serialized.StartsWith('M') || serialized.StartsWith("-M")
                || serialized.StartsWith('D') || serialized.StartsWith("-D")
                || serialized.StartsWith('h') || serialized.StartsWith("-h")
                || serialized.StartsWith('m') || serialized.StartsWith("-m")
                || serialized.StartsWith('s') || serialized.StartsWith("-str")
                || serialized.StartsWith('f') || serialized.StartsWith("-g"))
            {
                return Time.Parse(serialized, this);
            }

            bool startNumeric = (serialized[0] >= '0' && serialized[0] <= '9') || serialized[0] == '-' || serialized[0] == '.';
            if (startNumeric)
            {
                if (serialized.Contains('.'))
                    return Float.Parse(serialized, this);
                else
                    return Int.Parse(serialized, this);
            }

            // Invalid string.
            throw new ArgumentException($"Invalid string '{serialized}'");
        }
    }
}
