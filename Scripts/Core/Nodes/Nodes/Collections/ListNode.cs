namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A list serializer node.
    /// </summary>
    public class ListNode : INode
    {
        /* Public properties. */
        public INode[] Elements { get; set; }

        /* Constructors. */
        public ListNode(INode[] elements)
        {
            Elements = elements;
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
            Elements = null;
        }

        public void ClearRecursive()
        {
            // Clear child nodes.
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].ClearRecursive();
            }

            // Clear this node.
            Clear();
        }
    }
}