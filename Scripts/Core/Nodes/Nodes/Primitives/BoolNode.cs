namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A boolean serializer node.
    /// </summary>
    public class BoolNode : ValueNode<bool>
    {
        /* Constructors. */
        public BoolNode() : this(false) { }

        public BoolNode(bool value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "bool: " + Name;
        }
    }
}