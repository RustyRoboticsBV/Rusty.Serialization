using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A decimal converter.
    /// </summary>
    public sealed class DecimalConverter : TypedConverter<decimal, DecimalNode>
    {
        /* Protected methods. */
        protected override DecimalNode CreateNode2(decimal obj, CreateNodeContext context) => new DecimalNode(obj);
        protected override decimal CreateObject2(DecimalNode node, CreateObjectContext context) => (decimal)node.Value;
    }
}