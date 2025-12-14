using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An integer converter.
    /// </summary>
    public sealed class IntConverter : ValueConverter<int, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(int obj, CreateNodeContext context) => new(obj);
        protected override int CreateObject(IntNode node, CreateObjectContext context) => (int)node.Value;
    }
}