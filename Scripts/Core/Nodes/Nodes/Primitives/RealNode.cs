namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A real number serializer node.
    /// </summary>
    public readonly struct RealNode : INode
    {
        /* Fields. */
        private readonly decimal value;

        /* Public properties. */
        public readonly decimal Value => value;

        /* Constructors. */
        public RealNode(decimal value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return "real: " + value;
        }
    }
}