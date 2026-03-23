namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable collection node.
    /// </summary>
    public interface ICollectionNode : IContainerNode
    {
        /* Public properties. */
        /// <summary>
        /// The number of child nodes that this node holds.
        /// </summary>
        public int Count { get; }

        /* Public methods. */
        /// <summary>
        /// Get the value child node at some index.
        /// </summary>
        public INode GetValueAt(int index);
    }
}