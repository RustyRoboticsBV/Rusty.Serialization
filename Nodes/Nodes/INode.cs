namespace Rusty.Serialization.Nodes;

/// <summary>
/// A serializable node.
/// </summary>
public interface INode
{
    /* Public methods. */
    /// <summary>
    /// Serialize this node into text that can be safely written to a data file.
    /// </summary>
    public string Serialize();
}