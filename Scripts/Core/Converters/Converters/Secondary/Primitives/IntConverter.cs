using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An integer converter.
    /// </summary>
    public sealed class IntConverter : Converter<int, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(int obj, CreateNodeContext context) => new IntNode(obj);
        protected override int CreateObject(IntNode node, CreateObjectContext context) => (int)node.Name;
    }
}