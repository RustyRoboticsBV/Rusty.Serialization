using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A timestamp node value.
    /// </summary>
    public readonly struct TimestampValue : IEquatable<TimestampValue>
    {
        /* Fields */
        public readonly IntValue year;
        public readonly IntValue month;
        public readonly IntValue day;
        public readonly IntValue hour;
        public readonly IntValue minute;
        public readonly FloatValue second;
        public readonly TimeZone timezone;

        public readonly struct TimeZone
        {
            public readonly bool sign;
            public readonly IntValue hours;
            public readonly IntValue minutes;

            public static TimeZone UTC0 => new TimeZone(true, 0, 0);
            public bool IsZero => hours.IsZero && minutes.IsZero;

            public TimeZone(bool sign, IntValue hours, IntValue minutes)
            {
                if (hours.IsNegative)
                    throw new ArgumentException("Timezone hours may not be negative.");
                if (minutes.IsNegative)
                    throw new ArgumentException("Timezone minutes may not be negative.");

                this.sign = sign;
                this.hours = hours;
                this.minutes = minutes;
            }

            public override string ToString()
            {
                if (sign)
                    return $"+{hours}:{minutes}";
                else
                    return $"-{hours}:{minutes}";
            }
            public override int GetHashCode() => HashCode.Combine(sign, hours, minutes);
        }

        /* Constructors */
        public TimestampValue(IntValue year, IntValue month, IntValue day, IntValue hour, IntValue minute, FloatValue second)
            : this(year, month, day, hour, minute, second, TimeZone.UTC0) { }


        public TimestampValue(IntValue year, IntValue month, IntValue day, IntValue hour, IntValue minute, FloatValue second, TimeZone timezone)
        {
            if (month.IsNegative)
                throw new ArgumentException("Month may not be negative.");
            if (day.IsNegative)
                throw new ArgumentException("Day may not be negative.");
            if (hour.IsNegative)
                throw new ArgumentException("Hour may not be negative.");
            if (minute.IsNegative)
                throw new ArgumentException("Minute may not be negative.");
            if (second.negative)
                throw new ArgumentException("Second may not be negative.");

            this.year = year;
            this.month = month;
            this.day = day;

            this.hour = hour;
            this.minute = minute;
            this.second = second;

            this.timezone = timezone;
        }

        /* Public methods */
        public override string ToString()
        {
            if (timezone.IsZero)
                return $"{year}/{month}/{day},{hour}:{minute}:{second}";
            else
                return $"{year}/{month}/{day},{hour}:{minute}:{second}{timezone}";
        }
        public override int GetHashCode() => HashCode.Combine(year, month, day, hour, minute, second, timezone);
        public override bool Equals(object obj) => obj is TimestampValue other && Equals(other);
        public bool Equals(TimestampValue other)
            => year == other.year && month == other.month && day == other.day
            && hour == other.hour && minute == other.minute && second == other.second
            && timezone.Equals(other.timezone);

        public static TimestampValue Parse(string str)
        {
            ReadOnlySpan<char> span = str.AsSpan();

            int endOfYear = str.IndexOf('/');
            int endOfMonth = str.IndexOf('/', endOfYear + 1);
            int endOfDay = str.IndexOf(',', endOfMonth + 1);
            int endOfHour = str.IndexOf(':', endOfDay + 1);
            int endOfMinute = str.IndexOf(':', endOfHour + 1);

            if (str.IndexOf('-', 1) != -1)
                throw new FormatException($"Bad timestamp \"{str}\": only the first character may be a minus sign.");

            if (endOfYear < 0 || endOfMonth < 0 || endOfDay < 0 || endOfHour < 0 || endOfMinute < 0)
                throw new FormatException($"Bad timestamp \"{str}\": invalid separators.");

            IntValue year = IntValue.Parse(span.Slice(0, endOfYear));
            IntValue month = IntValue.Parse(span.Slice(endOfYear + 1, endOfMonth - endOfYear - 1));
            IntValue day = IntValue.Parse(span.Slice(endOfMonth + 1, endOfDay - endOfMonth - 1));
            IntValue hour = IntValue.Parse(span.Slice(endOfDay + 1, endOfHour - endOfDay - 1));
            IntValue minute = IntValue.Parse(span.Slice(endOfHour + 1, endOfMinute - endOfHour - 1));
            FloatValue second = FloatValue.Parse(span.Slice(endOfMinute + 1));

            return new TimestampValue(year, month, day, hour, minute, second);
        }
    }
}