#if NET5_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Half converter.
    /// </summary>
    public sealed class HalfConverter : ValueConverter<Half, RealNode>
    {
        /* Protected methods. */
        protected override RealNode ConvertValue(Half obj, IConverterScheme scheme, SymbolTable table) => new((decimal)obj);
        protected override Half DeconvertValue(RealNode node, IConverterScheme scheme, ParsingTable table) => (Half)node.Value.ToSingle();
    }
}
#endif