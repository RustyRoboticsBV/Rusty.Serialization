#if NET6_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

// TODO: Account for fractional seconds.

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date converter.
    /// </summary>
    public class DateOnlyConverter : Core.Conversion.Converter<DateOnly, TimestampNode>
    {
        /* Protected method. */
        protected override TimestampNode CreateNode(DateOnly obj, CreateNodeContext context)
        {
            return new TimestampNode(
                Math.Abs(obj.Year), Math.Abs(obj.Month), Math.Abs(obj.Day),
                0, 0, 0f
            );
        }

        protected override DateOnly CreateObject(TimestampNode node, CreateObjectContext context)
        {
            return new DateOnly((int)node.Value.year, (int)node.Value.month, (int)node.Value.day);
        }
    }
}
#endif