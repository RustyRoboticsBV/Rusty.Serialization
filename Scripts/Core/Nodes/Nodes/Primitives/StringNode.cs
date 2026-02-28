namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string serializer node.
    /// </summary>
    public class StringNode : ValueNode<string>
    {
        /* Constructors. */
        public StringNode() : base() { }

        public StringNode(string value) : base(value){}

        /* Public methods. */
        public override string ToString()
        {
            return "string: " + (Value ?? "(null)");
        }

        /* Casting operators. */
        public static implicit operator TimestampNode(StringNode node)
        {
            return new TimestampNode(TimestampValue.Parse(node.Value));
        }
    }
}