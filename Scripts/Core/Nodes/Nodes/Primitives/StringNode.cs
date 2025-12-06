namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string serializer node.
    /// </summary>
    public class StringNode : INode
    {
        /* Public properties. */
        public INode Parent { get; set; }
        public string Value { get; set; }

        /* Constructors. */
        public StringNode(string value)
        {
            Value = value ?? "";
        }

        /* Public methods. */
        public override string ToString()
        {
            return "string: " + (Value ?? "(null)");
        }

        public void Clear()
        {
            Parent = null;
            Value = null;
        }
    }
}