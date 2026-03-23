namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable collection node.
    /// </summary>
    public interface ITreeElement
    {
        /* Public methods. */
        /// <summary>
        /// Clear this node and all child nodes to their default state.
        /// </summary>
        public void Clear();
    }
}