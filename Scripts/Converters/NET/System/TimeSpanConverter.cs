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
            return new(false,
                0, 0, (ulong)Math.Abs(obj.Days),
                (ulong)Math.Abs(obj.Hours), (ulong)Math.Abs(obj.Minutes), (ulong)Math.Abs(obj.Seconds), (ulong)Math.Abs(obj.Milliseconds));
        }

        protected override TimeSpan CreateObject(TimeNode node, CreateObjectContext context)
        {
            return new((int)node.Day,
                (int)node.Hour, (int)node.Minute, (int)node.Second, (int)node.Millisecond);
        }
    }
}