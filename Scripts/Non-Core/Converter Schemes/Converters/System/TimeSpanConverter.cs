using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.TimeSpan converter.
    /// </summary>
    public sealed class TimeSpanConverter : ValueConverter<TimeSpan, TimeNode>
    {
        /* Protected methods. */
        protected override TimeNode ConvertValue(TimeSpan obj, IConverterScheme scheme, SymbolTable table) => new(new(obj));
        protected override TimeSpan DeconvertValue(TimeNode node, IConverterScheme scheme, NodeTree tree)
            => new(node.Value.negative ? -(int)node.Value.day : (int)node.Value.day,
                (int)node.Value.hour, (int)node.Value.minute, (int)node.Value.second, (int)node.Value.millisecond);
    }
}