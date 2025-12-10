namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A list serializer node.
    /// </summary>
    public class ListNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public INode[] Elements { get; set; }

        /* Constructors. */
        public ListNode(int capacity) : this(new INode[capacity]) { }

        public ListNode(INode[] elements)
        {
            Elements = elements;

            for (int i = 0; i < Elements.Length; i++)
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

            if (Elements.Length == 0)
                return "list: (empty)";

            string str = "list:";
            for (int i = 0; i < Elements.Length; i++)
            {
                str += '\n' + PrintUtility.PrintChild(Elements[i], i == Elements.Length - 1);
            }
            return str;
        }

        public void Clear()
        {
            Parent = null;
            for (int i = 0; i < Elements.Length; i++)
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
            for (int i = 0; i < Elements.Length; i++)
            {
                if (Elements[i] == element)
                    return i;
            }
            return -1;
        }
    }
}