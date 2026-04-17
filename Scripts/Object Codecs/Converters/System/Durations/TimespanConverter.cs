using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A timespan converter.
    /// </summary>
    // TODO: handle fractional seconds.
    public sealed class TimeSpanConverter : DurationConverter<TimeSpan>
    {
        /* Protected methods. */
        protected override DurationValue ToDuration(TimeSpan obj)
        {
            return new DurationValue(obj.Ticks < 0, obj.Days, obj.Hours, obj.Minutes, obj.Seconds);
        }

        protected override TimeSpan FromDuration(DurationValue value)
        {
            TimeSpan obj = new TimeSpan((int)value.days, (int)value.hours, (int)value.minutes, (int)value.seconds);
            if (value.negative)
                obj = obj.Negate();
            return obj;
        }
    }
}