using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A ushort converter.
    /// </summary>
    public sealed class UshortConverter : Converter<ushort, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(ushort obj, CreateNodeContext context) => new(obj);
        protected override ushort CreateObject(IntNode node, CreateObjectContext context) => ushort.Parse(node.Value);
    }
}