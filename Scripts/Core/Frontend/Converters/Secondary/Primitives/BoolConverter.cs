using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A bool converter.
    /// </summary>
    public sealed class BoolConverter : TypedConverter<bool, BoolNode>
    {
        /* Protected methods. */
        protected override BoolNode CreateNode2(bool obj, CreateNodeContext context) => new BoolNode(obj);
        protected override bool CreateObject2(BoolNode node, CreateObjectContext context) => node.Value;
    }
}