namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An integer serializer node.
    /// </summary>
    public class IntNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public IntString Value { get; set; }

        /* Constructors. */
        public IntNode(IntString value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator IntNode(FloatNode real)
        {
            return new IntNode(real.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return "int: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = 0;
        }
    }
}