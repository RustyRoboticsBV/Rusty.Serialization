namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A list serializer node.
    /// </summary>
    public readonly struct ListNode : INode
    {
        /* Fields. */
        private readonly INode[] elements;

        /* Public properties. */
        public readonly INode[] Elements => elements;

        /* Constructors. */
        public ListNode(INode[] elements)
        {
            this.elements = elements;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            string str = "list: ";
            for (int i = 0; i < elements.Length; i++)
            {
                str += "\n- " + elements[i].ToString().Replace("\n", "\n  ");
            }
            return str;
        }
    }
}