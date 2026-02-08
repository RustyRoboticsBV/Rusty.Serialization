using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A ulong converter.
    /// </summary>
    public sealed class UlongConverter : Converter<ulong, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(ulong obj, CreateNodeContext context) => new IntNode(obj);
        protected override ulong CreateObject(IntNode node, CreateObjectContext context) => (ulong)node.Value;
    }
}