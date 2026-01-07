namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A serializable node that represents a text value.
    /// </summary>
    public interface ITextNode : INode
    {
        /* Public properties. */
        public string Value { get; set; }
    }
}