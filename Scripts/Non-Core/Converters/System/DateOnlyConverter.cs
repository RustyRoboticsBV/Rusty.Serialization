#if NET6_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.DateOnly converter.
    /// </summary>
    public sealed class DateOnlyConverter : ValueConverter<DateOnly, TimeNode>
    {
        /* Protected methods. */
        protected override TimeNode ConvertValue(DateOnly obj, IConverterScheme scheme)
            => new(new(obj.Year, obj.Month, obj.Day, 0, 0, 0, 0));
        protected override DateOnly DeconvertValue(TimeNode node, IConverterScheme scheme)
            => new((int)node.Value.year, (int)node.Value.month, (int)node.Value.day);
    }
}
#endif