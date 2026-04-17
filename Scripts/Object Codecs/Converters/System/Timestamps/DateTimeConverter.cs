using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A date/time converter.
    /// </summary>
    // TODO: handle fractional seconds.
    public sealed class DateTimeConverter : TimestampConverter<DateTime>
    {
        /* Protected methods. */
        protected override TimestampValue ToTimestamp(DateTime obj)
        {
            return new TimestampValue(obj.Year, obj.Month, obj.Day, obj.Hour, obj.Minute, obj.Second);
        }

        protected override DateTime FromTimestamp(TimestampValue value)
        {
            return new DateTime(
                (int)value.year, (int)value.month, (int)value.day,
                (int)value.hour, (int)value.minute, (int)value.second
            );
        }
    }
}