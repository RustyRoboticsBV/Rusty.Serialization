namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string serializer node.
    /// </summary>
    public readonly struct StringNode : INode
    {
        /* Fields. */
        private readonly string value;

        /* Public properties. */
        public readonly string Value => value;

        /* Constructors. */
        public StringNode(string value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return "string: " + value;
        }
    }
}