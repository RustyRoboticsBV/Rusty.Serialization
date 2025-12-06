namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable collection node.
    /// </summary>
    public interface ICollectionNode : INode
    {
        /* Public methods. */
        /// <summary>
        /// Wrap a child node inside of some other node.
        /// </summary>
        public void WrapChild(INode child, INode wrapper);
    }
}