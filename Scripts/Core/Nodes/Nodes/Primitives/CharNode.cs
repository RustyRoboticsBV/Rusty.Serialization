namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A character serializer node.
    /// </summary>
    public readonly struct CharNode : INode
    {
        /* Fields. */
        private readonly char value;

        /* Public properties. */
        public readonly char Value => value;

        /* Constructors. */
        public CharNode(char value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return "char: " + value;
        }
    }
}