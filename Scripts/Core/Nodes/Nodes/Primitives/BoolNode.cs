namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A boolean serializer node.
    /// </summary>
    public class BoolNode : ValueNode<BoolValue>
    {
        /* Constructors. */
        public BoolNode() : this(false) { }

        public BoolNode(BoolValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "bool: " + Value;
        }
    }
}