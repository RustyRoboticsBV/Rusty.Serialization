namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A decimal number serializer node.
    /// </summary>
    public sealed class DecimalNode : ValueNode<DecimalValue>
    {
        /* Constructors. */
        public DecimalNode() : base() { }

        public DecimalNode(DecimalValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "decimal: " + Value;
        }
    }
}