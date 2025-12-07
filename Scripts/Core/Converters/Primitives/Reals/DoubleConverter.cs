using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A double converter.
    /// </summary>
    public sealed class DoubleConverter : ValueConverter<double, RealNode>
    {
        /* Protected methods. */
        protected override RealNode ConvertValue(double obj, IConverterScheme scheme, SymbolTable table) => new(PeterO.Numbers.EDecimal.FromDouble(obj));
        protected override double DeconvertValue(RealNode node, IConverterScheme scheme, NodeTree tree) => (double)node.Value;
    }
}