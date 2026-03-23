namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A base class for serializer nodes without any child nodes.
    /// </summary>
    public abstract class LeafNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }

        /* Constructors. */
        public LeafNode() { }

        /* Public methods. */
        public virtual void Clear()
        {
            Parent = null;
        }
    }
}