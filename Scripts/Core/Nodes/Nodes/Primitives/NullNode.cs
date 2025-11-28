namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A null serializer node.
    /// </summary>
    public readonly struct NullNode : INode
    {
        /* Public methods. */
        public override readonly string ToString()
        {
            return "null";
        }
    }
}