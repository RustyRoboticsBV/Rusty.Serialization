#if NET6_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date converter.
    /// </summary>
    public class DateOnlyConverter : Core.Converters.Converter<DateOnly, TimeNode>
    {
        /* Protected method. */
        protected override TimeNode CreateNode(DateOnly obj, CreateNodeContext context)
        {
            return new(false,
                (ulong)Math.Abs(obj.Year), (ulong)Math.Abs(obj.Month), (ulong)Math.Abs(obj.Day),
                0, 0, 0, 0);
        }

        protected override DateOnly CreateObject(TimeNode node, CreateObjectContext context)
        {
            return new((int)node.Year, (int)node.Month, (int)node.Day);
        }
    }
}
#endif