using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET timespan converter.
    /// </summary>
    public class TimeSpanConverter : Core.Conversion.Converter<TimeSpan, DurationNode>
    {
        /* Protected method. */
        protected override DurationNode CreateNode(TimeSpan obj, CreateNodeContext context)
        {
            bool negative = obj.Ticks < 0;
            if (negative)
            obj = obj.Negate();

            int days = obj.Days;
            int hours = obj.Hours;
            int minutes = obj.Minutes;
            double seconds = obj.Seconds + obj.Milliseconds / 1000.0;
            return new DurationNode(negative, days, hours, minutes, seconds);
        }

        protected override TimeSpan CreateObject(DurationNode node, CreateObjectContext context)
        {
            double totalSeconds = (double)node.Value.seconds
                                + (long)node.Value.minutes * 60
                                + (long)node.Value.hours * 60 * 60
                                + (long)node.Value.days * 60 * 60 * 24;

            if (node.Value.negative)
                totalSeconds = -totalSeconds;

            return TimeSpan.FromSeconds(totalSeconds);
        }
    }
}