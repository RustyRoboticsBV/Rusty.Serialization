using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET time span converter.
    /// </summary>
    public class TimeSpanConverter : Core.Converters.Converter<TimeSpan, TimeNode>
    {
        /* Protected method. */
        protected override TimeNode CreateNode(TimeSpan obj, CreateNodeContext context)
        {
            TimeSpan abs = obj.Duration();
            return new(obj.Ticks < 0,
                0, 0, (ulong)abs.Days,
                (ulong)abs.Hours, (ulong)abs.Minutes, (ulong)abs.Seconds,
                (ulong)abs.Milliseconds, (ulong)abs.Ticks % TimeSpan.TicksPerMillisecond * 100);
        }

        protected override TimeSpan CreateObject(TimeNode node, CreateObjectContext context)
        {
            TimeSpan obj = new((int)node.Day,
                (int)node.Hour, (int)node.Minute, (int)node.Second,
                (int)node.Millisecond);

            obj += TimeSpan.FromTicks((long)node.Nanosecond / 100);

            return node.Negative ? obj.Negate() : obj;
        }
    }
}