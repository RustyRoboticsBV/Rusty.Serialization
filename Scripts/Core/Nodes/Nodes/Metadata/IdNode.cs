namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An ID serializer node.
    /// </summary>
    public class IdNode : INode
    {
        /* Public properties. */
        public INode Parent { get; set; }
        public ulong Index { get; set; }
        public INode Value { get; set; }

        /* Constructors. */
        public IdNode(ulong index, INode value)
        {
            Index = index;
            Value = value;

            if (Value != null)
                Value.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "ID: " + Index + "\n" + PrintUtility.PrintChild(Value);
        }

        public void Clear()
        {
            Parent = null;
            Index = 0;
            Value.Clear();
            Value = null;
        }
    }
}