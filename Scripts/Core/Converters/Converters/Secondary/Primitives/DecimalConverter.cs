using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A decimal converter.
    /// </summary>
    public sealed class DecimalConverter : Converter<decimal, RealNode>
    {
        /* Protected methods. */
        protected override RealNode CreateNode(decimal obj, CreateNodeContext context) => new(obj);
        protected override decimal CreateObject(RealNode node, CreateObjectContext context) => (decimal)node.Value;
    }
}