using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A uint converter.
    /// </summary>
    public sealed class UintConverter : Converter<uint, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(uint obj, CreateNodeContext context) => new(obj);
        protected override uint CreateObject(IntNode node, CreateObjectContext context) => (uint)node.Value;
    }
}