using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A ulong converter.
    /// </summary>
    public sealed class UlongConverter : ValueConverter<ulong, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(ulong obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override ulong DeconvertValue(IntNode node, IConverterScheme scheme, ParsingTable table) => (ulong)node.Value;
    }
}