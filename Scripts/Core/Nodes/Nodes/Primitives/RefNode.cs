namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A reference serializer node.
    /// </summary>
    public readonly struct RefNode : INode
    {
        /* Fields. */
        private readonly ulong id;

        /* Public properties. */
        public readonly ulong ID => id;

        /* Constructors. */
        public RefNode(ulong id)
        {
            this.id = id;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return "ref: " + id;
        }
    }
}