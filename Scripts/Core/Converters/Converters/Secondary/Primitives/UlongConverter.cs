using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A ulong converter.
    /// </summary>
    public sealed class UlongConverter : Converter<ulong, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(ulong obj, CreateNodeContext context) => new(obj);
        protected override ulong CreateObject(IntNode node, CreateObjectContext context) => ulong.Parse(node.Value);
    }
}