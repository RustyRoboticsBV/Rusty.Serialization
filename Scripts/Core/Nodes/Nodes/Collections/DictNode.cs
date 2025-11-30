using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A dictionary serializer node.
    /// </summary>
    public readonly struct DictNode : INode
    {
        /* Fields. */
        private readonly KeyValuePair<INode, INode>[] pairs;

        /* Public properties. */
        public readonly KeyValuePair<INode, INode>[] Pairs => pairs;

        /* Constructors. */
        public DictNode(KeyValuePair<INode, INode>[] pairs)
        {
            this.pairs = pairs ?? new KeyValuePair<INode, INode>[0];
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            if (pairs.Length == 0)
                return "dictionary: (empty)";

            string str = "dictionary:";
            foreach (var kv in pairs)
            {
                string keyStr = kv.Key.ToString().Replace("\n", "\n ");
                string valStr = kv.Value.ToString().Replace("\n", "\n ");
                str += $"\n- {keyStr} => {valStr}";
            }
            return str;
        }
    }
}