namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A null serializer node.
    /// </summary>
    public sealed class NullNode : LeafNode
    {
        /* Public methods. */
        public override string ToString()
        {
            return "null";
        }
    }
}