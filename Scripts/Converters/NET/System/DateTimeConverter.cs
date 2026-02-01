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
                obj.Year, (byte)obj.Month, (byte)obj.Day,
                (byte)obj.Hour, (byte)obj.Minute, seconds
            ));
        }

        protected override DateTime CreateObject(TimeNode node, CreateObjectContext context)
        {
            // Convert fractional seconds into ticks.
            double totalSeconds = (double)node.Value.second;
            int intSeconds = (int)Math.Floor(totalSeconds);
            double fractionSeconds = totalSeconds - intSeconds;
            long fractionTicks = (long)Math.Round(fractionSeconds * TimeSpan.TicksPerSecond);

            // Create new object.
            try
            {
                return new DateTime(
                    (int)node.Value.year, (int)node.Value.month, (int)node.Value.day,
                    (int)node.Value.hour, (int)node.Value.minute, intSeconds
                ).AddTicks(fractionTicks);
            }
            catch
            {
                throw new FormatException("Cannot create valid date/time from " + node);
            }
        }
    }
}