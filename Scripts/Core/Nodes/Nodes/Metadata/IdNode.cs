namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An ID serializer node.
    /// </summary>
    public class IdNode : INode
    {
        /* Public properties. */
        public ulong Index { get; set; }
        public INode Value { get; set; }

        /* Constructors. */
        public IdNode(ulong index, INode value)
        {
            Index = index;
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "index: " + Index + "\n" + PrintUtility.PrintChild(Value);
        }

        public void Clear()
        {
            Index = 0;
            Value = null;
        }

        public void ClearRecursive()
        {
            Value?.ClearRecursive();
            Clear();
        }

        /// <summary>
        /// Wrap the value node inside of an ID node.
        /// </summary>
        public void WrapId(ulong id)
        {
            IdNode node = new(id, Value);
            Value = node;
        }
    }
}