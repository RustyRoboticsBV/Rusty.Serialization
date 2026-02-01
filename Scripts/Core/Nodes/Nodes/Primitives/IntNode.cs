namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An integer serializer node.
    /// </summary>
    public class IntNode : ValueNode<IntString>
    {
        /* Constructors. */
        public IntNode() : base() { }

        public IntNode(IntString value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "int: " + Value;
        }
    }
}