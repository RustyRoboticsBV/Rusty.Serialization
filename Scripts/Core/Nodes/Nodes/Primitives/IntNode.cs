namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An integer serializer node.
    /// </summary>
    public class IntNode : ValueNode<IntValue>
    {
        /* Constructors. */
        public IntNode() : base() { }

        public IntNode(IntValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "int: " + Value;
        }
    }
}