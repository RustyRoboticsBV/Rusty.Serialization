using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A long converter.
    /// </summary>
    public sealed class LongConverter : ValueConverter<long, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(long obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override long DeconvertValue(IntNode node, IConverterScheme scheme, ParsingTable table) => (long)node.Value;
    }
}