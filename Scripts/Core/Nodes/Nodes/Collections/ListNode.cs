using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A list serializer node.
    /// </summary>
    public class ListNode : ICollectionNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public List<INode> Elements { get; set; }
        public int Count => Elements.Count;

        /* Constructors. */
        public ListNode()
        {
            Elements = new List<INode>();
        }

        public ListNode(int capacity) : this(new INode[capacity]) { }

        public ListNode(params INode[] elements) : this(new List<INode>(elements)) { }

        public ListNode(List<INode> elements)
        {
            Elements = new List<INode>(elements);

            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] != null)
                    Elements[i].Parent = this;
            }
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Elements == null)
                return "list: (null)";

            if (Elements.Count == 0)
                return "list: (empty)";

            string str = "list:";
            for (int i = 0; i < Elements.Count; i++)
            {
                str += '\n' + PrintUtility.PrintChild(Elements[i], i == Elements.Count - 1);
            }
            return str;
        }

        public void Clear()
        {
            Parent = null;
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Clear();
            }
            Elements = null;
        }

        /// <summary>
        /// Get the index of an element node. Returns -1 if the node is not an element.
        /// </summary>
        public int IndexOf(INode element)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] == element)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Add a value at the end of the list.
        /// </summary>
        public void AddValue(INode value)
        {
            Elements.Add(null);
            SetValueAt(Count - 1, value);
        }

        public void SetValueAt(int index, INode value)
        {
            Elements[index] = value;
            value.Parent = this;
        }

        public INode GetValueAt(int index)
        {
            return Elements[index];
        }

        public void ReplaceChild(INode oldChild, INode newChild)
        {
            int index = IndexOf(oldChild);
            if (index != -1)
            {
                if (oldChild.Parent == this)
                    oldChild.Parent = null;
                newChild.Parent = this;
                Elements[index] = newChild;
                return;
            }
            throw new ArgumentException($"'{oldChild}' was not a child of '{this}'.");
        }
    }
}