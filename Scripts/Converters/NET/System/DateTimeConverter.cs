using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date/time converter.
    /// </summary>
    public class DateTimeConverter : Core.Converters.Converter<DateTime, TimeNode>
    {
        /* Protected method. */
        protected override TimeNode CreateNode(DateTime obj, CreateNodeContext context)
        {
            return new(false,
                (ulong)obj.Year, (ulong)obj.Month, (ulong)obj.Day,
                (ulong)obj.Hour, (ulong)obj.Minute, (ulong)obj.Second,
                (ulong)obj.Millisecond, (ulong)obj.Ticks % TimeSpan.TicksPerMillisecond * 100);
        }

        protected override DateTime CreateObject(TimeNode node, CreateObjectContext context)
        {
            DateTime obj = new(
                (int)node.Year, (int)node.Month, (int)node.Day,
                (int)node.Hour, (int)node.Minute, (int)node.Second,
                (int)node.Millisecond
            );

            long nanosecondTicks = (long)node.Nanosecond / 100;
            return obj.AddTicks(nanosecondTicks);
        }
    }
}