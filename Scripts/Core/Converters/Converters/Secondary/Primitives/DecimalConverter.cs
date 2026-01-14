using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A decimal converter.
    /// </summary>
    public sealed class DecimalConverter : Converter<decimal, CurrencyNode>
    {
        /* Protected methods. */
        protected override CurrencyNode CreateNode(decimal obj, CreateNodeContext context) => new(obj);
        protected override decimal CreateObject(CurrencyNode node, CreateObjectContext context) => decimal.Parse(node.Value);
    }
}