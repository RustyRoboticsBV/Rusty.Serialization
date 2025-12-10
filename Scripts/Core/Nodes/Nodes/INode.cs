namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable node.
    /// </summary>
    public interface INode : ITreeElement
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
    }
}