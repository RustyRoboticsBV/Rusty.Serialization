namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable node.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Clear this node and all child nodes to their default state.
        /// </summary>
        public void ClearRecursive();

        /// <summary>
        /// Clear this node to its default state. This does not touch child nodes.
        /// </summary>
        public void Clear();
    }
}