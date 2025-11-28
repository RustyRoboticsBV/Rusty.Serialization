namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A boolean serializer node.
    /// </summary>
    public readonly struct BoolNode : INode
    {
        /* Fields. */
        private readonly bool value;

        /* Public properties. */
        public readonly bool Value => value;

        /* Constructors. */
        public BoolNode(bool value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return "bool: " + value;
        }
    }
}