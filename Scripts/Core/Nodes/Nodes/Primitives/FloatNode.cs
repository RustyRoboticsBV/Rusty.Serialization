namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A floating-point number serializer node.
    /// </summary>
    public sealed class FloatNode : ValueNode<FloatValue>
    {
        /* Constructors. */
        public FloatNode() : base() { }

        public FloatNode(FloatValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "float: " + Value;
        }

        /* Conversion oeprators. */
        public static explicit operator IntNode(FloatNode node)
        {
            return new IntNode(node.Value.negative ? -node.Value.integral : node.Value.integral);
        }
    }
}