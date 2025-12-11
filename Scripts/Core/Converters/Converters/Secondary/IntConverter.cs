using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A reference tuple type converter.
    /// </summary>
    public sealed class IntConverter : ValueConverter<int, IntNode>
    {
        /* Protected methods. */
        protected override int CreateObject(IntNode node, CreateObjectContext context)
        {
            return (int)node.Value;
        }

        protected override IntNode CreateNode(int obj, CreateNodeContext context)
        {
            return new(obj);
        }
    }
}