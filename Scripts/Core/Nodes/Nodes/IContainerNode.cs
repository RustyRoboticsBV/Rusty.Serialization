namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable node that can contain other node(s).
    /// </summary>
    public interface IContainerNode : INode
    {
        /* Public methods. */
        /// <summary>
        /// Swap out a child node with another node.
        /// </summary>
        public void ReplaceChild(INode oldChild, INode newChild);
    }
}