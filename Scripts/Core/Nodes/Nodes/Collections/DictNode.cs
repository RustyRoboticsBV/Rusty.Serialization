using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A dictionary serializer node.
    /// </summary>
    public class DictNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public KeyValuePair<INode, INode>[] Pairs { get; set; }

        /* Constructors. */
        public DictNode(int capacity) : this(new KeyValuePair<INode, INode>[capacity]) { }

        public DictNode(KeyValuePair<INode, INode>[] pairs)
        {
            Pairs = pairs ?? new KeyValuePair<INode, INode>[0];

            for (int i = 0; i < Pairs.Length; i++)
            {
                if (Pairs[i].Key != null)
                    Pairs[i].Key.Parent = this;
                if (Pairs[i].Value != null)
                    Pairs[i].Value.Parent = this;
            }
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
            Parent = null;
            for (int i = 0; i < Pairs.Length; i++)
            {
                Pairs[i].Key.Clear();
                Pairs[i].Value.Clear();
            }
            Pairs = null;
        }

        /// <summary>
        /// Get the index of a key node. Returns -1 if the node is not a key.
        /// </summary>
        public int IndexOfKey(INode key)
        {
            for (int i = 0; i < Pairs.Length; i++)
            {
                if (Pairs[i].Key == key)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Get the index of a value node. Returns -1 if the node is not a value.
        /// </summary>
        public int IndexOfValue(INode value)
        {
            for (int i = 0; i < Pairs.Length; i++)
            {
                if (Pairs[i].Value == value)
                    return i;
            }
            return -1;
        }
    }
}