namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A real number serializer node.
    /// </summary>
    public class RealNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public RealString Value { get; set; }

        /* Constructors. */
        public RealNode(RealString value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator RealNode(IntNode real)
        {
            return new RealNode(real.Value);
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