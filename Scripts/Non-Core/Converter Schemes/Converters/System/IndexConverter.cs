using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Index converter.
    /// </summary>
    public sealed class IndexConverter : ValueConverter<Index, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(Index obj, IConverterScheme scheme) => new(obj.Value);
        protected override Index DeconvertValue(IntNode node, IConverterScheme scheme) => (int)node.Value;
    }
}