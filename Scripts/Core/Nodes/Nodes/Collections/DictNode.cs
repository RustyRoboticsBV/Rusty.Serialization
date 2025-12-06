using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A dictionary serializer node.
    /// </summary>
    public class DictNode : INode
    {
        /* Public properties. */
        public KeyValuePair<INode, INode>[] Pairs { get; set; }

        /* Constructors. */
        public DictNode(KeyValuePair<INode, INode>[] pairs)
        {
            Pairs = pairs ?? new KeyValuePair<INode, INode>[0];
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Pairs == null)
                return "dictionary: (null)";

            if (Pairs.Length == 0)
                return "dictionary: (empty)";

            string str = "dictionary:";
            for (int i = 0; i < Pairs.Length; i++)
            {
                str += '\n' + PrintUtility.PrintPair(Pairs[i].Key, Pairs[i].Value, i == Pairs.Length - 1);
            }
            return str;
        }

        public void Clear()
        {
            Pairs = null;
        }

        public void ClearRecursive()
        {
            // Clear child nodes.
            for (int i = 0; i < Pairs.Length; i++)
            {
                Pairs[i].Key.ClearRecursive();
                Pairs[i].Value.ClearRecursive();
            }

            // Clear this node.
            Clear();
        }
    }
}