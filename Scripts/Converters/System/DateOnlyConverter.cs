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
        protected override TimeNode Convert(DateOnly obj, Context context)
            => new(new(obj.Year, obj.Month, obj.Day, 0, 0, 0, 0));
        protected override DateOnly Deconvert(TimeNode node, Context context)
            => new((int)node.Value.year, (int)node.Value.month, (int)node.Value.day);
    }
}
#endif