using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A float converter.
    /// </summary>
    public sealed class FloatConverter : ValueConverter<float, RealNode>
    {
        /* Protected methods. */
        protected override RealNode ConvertValue(float obj, IConverterScheme scheme, SymbolTable table) => new(PeterO.Numbers.EDecimal.FromSingle(obj));
        protected override float DeconvertValue(RealNode node, IConverterScheme scheme, NodeTree tree) => (float)node.Value;
    }
}