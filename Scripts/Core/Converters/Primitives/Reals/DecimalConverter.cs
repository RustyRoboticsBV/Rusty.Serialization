using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A decimal converter.
    /// </summary>
    public sealed class DecimalConverter : ValueConverter<decimal, RealNode>
    {
        /* Protected methods. */
        protected override RealNode ConvertValue(decimal obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override decimal DeconvertValue(RealNode node, IConverterScheme scheme, ParsingTable table) => node.Value.ToDecimal();
    }
}