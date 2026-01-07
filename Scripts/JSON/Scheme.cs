using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
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
        private CSCD.BoolSerializer Bool { get; } = new();
        private StringSerializer String { get; } = new();
        private ListSerializer List { get; } = new();
        private ObjectSerializer Object { get; } = new();

        /* Public methods. */
        public string Serialize(NodeTree tree, bool prettyPrint = false)
        {
            return Serialize(tree.Root, prettyPrint);
        }

        public string Serialize(INode node, bool prettyPrint = false)
        {
            switch (node)
            {

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

            // Array.
            if (serialized.StartsWith('[') && serialized.EndsWith(']'))
                return new ListNode(ParseUtility.Split(

            // Object.

            // String.
            if (serialized.StartsWith('"') && serialized.EndsWith('"'))
                return StringSerializer.

            // Invalid string.
            throw new ArgumentException($"Invalid string '{serialized}'");
        }
    }
}
