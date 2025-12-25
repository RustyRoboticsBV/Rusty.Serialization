namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable node that can contain other node(s).
    /// </summary>
    public interface IMetadataNode : IContainerNode
    {
        /* Public properties. */
        /// <summary>
        /// The contained child node.
        /// </summary>
        public INode Value { get; }
    }
}