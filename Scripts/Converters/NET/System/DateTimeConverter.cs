using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date/time converter.
    /// </summary>
    public class DateTimeConverter : Core.Conversion.Converter<DateTime, TimeNode>
    {
        /* Protected method. */
        protected override TimeNode CreateNode(DateTime obj, CreateNodeContext context)
        {
            double seconds = obj.Second + (obj.Millisecond / 1000.0) + (obj.Ticks % TimeSpan.TicksPerMillisecond * 1e-7);

            return new TimeNode(new TimeValue(
                obj.Year, obj.Month, obj.Day,
                obj.Hour, obj.Minute, seconds
            ));
        }

        protected override DateTime CreateObject(TimeNode node, CreateObjectContext context)
        {
            // Convert fractional seconds into ticks.
            double totalSeconds = (double)node.Name.second;
            int intSeconds = (int)Math.Floor(totalSeconds);
            double fractionSeconds = totalSeconds - intSeconds;
            long fractionTicks = (long)Math.Round(fractionSeconds * TimeSpan.TicksPerSecond);

            // Create new object.
            try
            {
                return new DateTime(
                    (int)node.Name.year, (int)node.Name.month, (int)node.Name.day,
                    (int)node.Name.hour, (int)node.Name.minute, intSeconds
                ).AddTicks(fractionTicks);
            }
            catch
            {
                throw new FormatException("Cannot create valid date/time from " + node);
            }
        }
    }
}