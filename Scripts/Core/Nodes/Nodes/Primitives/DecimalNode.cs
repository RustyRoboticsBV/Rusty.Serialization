namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A decimal number serializer node.
    /// </summary>
    public class DecimalNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public RealString Value { get; set; }

        /* Constructors. */
        public DecimalNode(RealString value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator DecimalNode(IntNode node)
        {
            return new DecimalNode(node.Value);
        }

        public static explicit operator DecimalNode(FloatNode node)
        {
            return new DecimalNode(node.Value);
        }

        public static explicit operator DecimalNode(StringNode node)
        {
            return new DecimalNode(node.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return "decimal: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = 0f;
        }
    }
}