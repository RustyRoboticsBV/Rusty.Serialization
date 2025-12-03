using System;
#if !UNITY_5_3_OR_NEWER
using System.Text.Json;
#endif
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    /// <summary>
    /// A JSON serialization scheme.
    /// </summary>
    public class Scheme : ISerializerScheme
    {
        /* Public properties. */
        public bool PrettyPrint { get; set; }
        public string Tab { get; set; } = "  ";

        /* Private properties. */
        private NullSerializer Null { get; } = new();
        private BoolSerializer Bool { get; } = new();
        private IntSerializer Int { get; } = new();
        private RealSerializer Real { get; } = new();
        private CharSerializer Char { get; } = new();
        private StringSerializer String { get; } = new();
        private ColorSerializer Color { get; } = new();
        private ListSerializer List { get; } = new();

        /* Public methods. */
        public string Serialize(INode node)
        {
            switch (node)
            {
                case NullNode n:
                    return Null.Serialize(node, this);
                case BoolNode b:
                    return Bool.Serialize(node, this);
                case IntNode i:
                    return Int.Serialize(node, this);
                case RealNode r:
                    return Real.Serialize(node, this);
                case CharNode c:
                    return Char.Serialize(node, this);
                case StringNode s:
                    return String.Serialize(node, this);
                case ColorNode col:
                    return Color.Serialize(node, this);
                case ListNode list:
                    return List.Serialize(node, this);
                default:
                    // TODO: remove this.
                    return "";
                    throw new ArgumentException($"Unknown node type '{node.GetType()}'.");
            }
            throw new NotImplementedException();
        }

        public INode Parse(string serialized)
        {
            throw new NotImplementedException();
        }
    }
}