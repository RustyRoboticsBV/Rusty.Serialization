namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A bytes serializer node.
    /// </summary>
    public class BytesNode : ValueNode<BytesString>
    {
        /* Constructors. */
        public BytesNode() : base() { }

        public BytesNode(BytesString value) : base(value) { }

        /* Conversion operators. */
        public static explicit operator BytesNode(StringNode str)
        {
            return new BytesNode(str.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"bytes: {Value}";
        }
    }
}