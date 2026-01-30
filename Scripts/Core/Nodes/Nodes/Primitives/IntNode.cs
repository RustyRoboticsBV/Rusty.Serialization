namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An integer serializer node.
    /// </summary>
    public class IntNode : ValueNode<IntString>
    {
        /* Constructors. */
        public IntNode(IntString value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "int: " + Value;
        }

        public override void Clear()
        {
            Parent = null;
            Value = 0;
        }
    }
}