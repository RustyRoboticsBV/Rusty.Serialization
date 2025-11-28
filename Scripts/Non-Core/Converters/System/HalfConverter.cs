using System;
using Rusty.Serialization.Core.Contexts;
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
        protected override RealNode ConvertValue(Half obj, IConverterScheme scheme) => new((decimal)obj);
        protected override Half DeconvertValue(RealNode node, IConverterScheme scheme) => (Half)node.Value;
    }
}