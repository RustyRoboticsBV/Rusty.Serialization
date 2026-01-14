namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A currency number serializer node.
    /// </summary>
    public class CurrencyNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public RealString Value { get; set; }

        /* Constructors. */
        public CurrencyNode(RealString value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator CurrencyNode(IntNode node)
        {
            return new CurrencyNode(node.Value);
        }

        public static explicit operator CurrencyNode(RealNode node)
        {
            return new CurrencyNode(node.Value);
        }

        public static explicit operator CurrencyNode(StringNode node)
        {
            return new CurrencyNode(node.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return "real: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = 0f;
        }
    }
}