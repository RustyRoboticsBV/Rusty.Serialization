namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A decimal number serializer node.
    /// </summary>
    public class DecimalNode : ValueNode<RealString>
    {
        /* Constructors. */
        public DecimalNode() : base() { }

        public DecimalNode(RealString value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "decimal: " + Value;
        }

        public override void Clear()
        {
            Parent = null;
            Value = 0f;
        }
    }
}