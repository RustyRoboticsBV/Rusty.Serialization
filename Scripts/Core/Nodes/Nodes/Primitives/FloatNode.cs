namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A floating-point number serializer node.
    /// </summary>
    public class FloatNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public RealString Value { get; set; }

        /* Constructors. */
        public FloatNode(RealString value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator FloatNode(IntNode node)
        {
            return new FloatNode(node.Value);
        }

        public static explicit operator FloatNode(StringNode node)
        {
            return new FloatNode(node.Value);
        }

        public static explicit operator FloatNode(DecimalNode node)
        {
            return new FloatNode(node.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return "float: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = 0f;
        }
    }
}