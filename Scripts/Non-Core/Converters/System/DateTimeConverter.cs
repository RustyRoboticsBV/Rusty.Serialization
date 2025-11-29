using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.DateTime converter.
    /// </summary>
    public sealed class DateTimeConverter : ValueConverter<DateTime, TimeNode>
    {
        /* Protected methods. */
        protected override TimeNode ConvertValue(DateTime obj, IConverterScheme scheme) => new(new(obj));
        protected override DateTime DeconvertValue(TimeNode node, IConverterScheme scheme)
            => new(node.Value.negative ? -(int)node.Value.year : (int)node.Value.year, (int)node.Value.month, (int)node.Value.day,
                (int)node.Value.hour, (int)node.Value.minute, (int)node.Value.second, (int)node.Value.millisecond);
    }
}