namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A floating-point number serializer node.
    /// </summary>
    public class FloatNode : ValueNode<FloatValue>
    {
        /* Constructors. */
        public FloatNode() : base() { }

        public FloatNode(FloatValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "float: " + Value;
        }
    }
}