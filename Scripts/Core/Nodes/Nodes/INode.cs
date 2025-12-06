namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable node.
    /// </summary>
    public interface INode
    {
        /* Public properties. */
        public INode Parent { get; set; }

        /* Public methods. */
        /// <summary>
        /// Clear this node and all child nodes to their default state.
        /// </summary>
        public void Clear();
    }
}