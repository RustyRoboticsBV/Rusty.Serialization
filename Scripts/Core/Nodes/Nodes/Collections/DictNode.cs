using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    using Pair = KeyValuePair<INode, INode>;

    /// <summary>
    /// A dictionary serializer node.
    /// </summary>
    public class DictNode : IDictionaryNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public List<Pair> Pairs { get; set; }
        public int Count => Pairs.Count;

        /* Constructors. */
        public DictNode() : this(new List<Pair>()) { }

        public DictNode(int capacity) : this(new Pair[capacity]) { }

        public DictNode(params Pair[] pairs) : this(new List<Pair>(pairs)) { }

        public DictNode(List<Pair> pairs)
        {
            Pairs = pairs ?? new List<Pair>();

            for (int i = 0; i < Count; i++)
            {
                if (Pairs[i].Key != null)
                    Pairs[i].Key.Parent = this;
                if (Pairs[i].Value != null)
                    Pairs[i].Value.Parent = this;
            }
        }

        /* Conversion operators. */
        public static explicit operator DictNode(ListNode list)
        {
            DictNode dict = new DictNode(list.Count);
            for (int i = 0; i < dict.Count; i++)
            {
                INode node = list.Elements[i];
                if (node is ObjectNode obj)
                    dict.Pairs[i] = new Pair(obj.GetValueAt(0), obj.GetValueAt(1));
            }
            return dict;
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Pairs == null)
                return "dictionary: (null)";

            if (Count == 0)
                return "dictionary: (empty)";

            string str = "dictionary:";
            for (int i = 0; i < Count; i++)
            {
                str += '\n' + PrintUtility.PrintPair(Pairs[i].Key, Pairs[i].Value, i == Count - 1);
            }
            return str;
        }

        public void Clear()
        {
            Parent = null;
            for (int i = 0; i < Count; i++)
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
            for (int i = 0; i < Count; i++)
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
            for (int i = 0; i < Count; i++)
            {
                if (Pairs[i].Value == value)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Add a new key-value pair.
        /// </summary>
        public void AddPair(INode key, INode value)
        {
            Pairs.Add(new Pair(key, value));
        }

        public INode GetKeyAt(int index)
        {
            return Pairs[index].Key;
        }

        public INode GetValueAt(int index)
        {
            return Pairs[index].Value;
        }

        public void ReplaceChild(INode oldChild, INode newChild)
        {
            int index = IndexOfKey(oldChild);
            if (index != -1)
            {
                if (oldChild.Parent == this)
                    oldChild.Parent = null;
                newChild.Parent = this;
                Pairs[index] = new(newChild, Pairs[index].Value);
                return;
            }

            index = IndexOfValue(oldChild);
            if (index != -1)
            {
                if (oldChild.Parent == this)
                    oldChild.Parent = null;
                newChild.Parent = this;
                Pairs[index] = new(Pairs[index].Key, newChild);
                return;
            }

            throw new ArgumentException($"'{oldChild}' was not a child of '{this}'.");
        }
    }
}