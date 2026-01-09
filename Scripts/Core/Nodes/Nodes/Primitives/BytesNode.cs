namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A bytes serializer node.
    /// </summary>
    public class BytesNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public BytesString Value { get; set; }

        /* Constructors. */
        public BytesNode(BytesString value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator BytesNode(StringNode str)
        {
            return new BytesNode(str.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"bytes: b{Value}";
        }

        public void Clear()
        {
            Parent = null;
            Value = "";
        }
    }
}