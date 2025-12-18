namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable dictionary node.
    /// </summary>
    public interface IDictionaryNode : ICollectionNode
    {
        /* Public methods. */
        /// <summary>
        /// Get the key child node at some index.
        /// </summary>
        public INode GetKeyAt(int index);
    }
}