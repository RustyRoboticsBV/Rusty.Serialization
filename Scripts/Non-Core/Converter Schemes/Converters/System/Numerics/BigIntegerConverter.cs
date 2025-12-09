using System.Numerics;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Numerics.BigInteger converter.
    /// </summary>
    public sealed class BigIntegerConverter : ValueConverter<BigInteger, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(BigInteger obj, IConverterScheme scheme, SymbolTable table) => new((decimal)obj);
        protected override BigInteger DeconvertValue(IntNode node, IConverterScheme scheme, ParsingTable table) => new(node.Value);
    }
}