using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A short converter.
    /// </summary>
    public sealed class ShortConverter : ValueConverter<short, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(short obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override short DeconvertValue(IntNode node, IConverterScheme scheme, ParsingTable table) => (short)node.Value;
    }
}