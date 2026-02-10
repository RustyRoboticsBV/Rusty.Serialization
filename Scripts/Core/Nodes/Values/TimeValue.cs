using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A time node value.
    /// </summary>
    public readonly struct TimeValue : IEquatable<TimeValue>
    {
        /* Fields */
        public readonly IntValue year;
        public readonly IntValue month;
        public readonly IntValue day;
        public readonly IntValue hour;
        public readonly IntValue minute;
        public readonly FloatValue second;

        /* Constructors */
        public TimeValue(IntValue year, IntValue month, IntValue day, IntValue hour, IntValue minute, FloatValue second)
        {
            if (second.negative)
                throw new ArgumentException("Second may not be negative.");

            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        /* Public methods */
        public override string ToString() => $"{year}/{month}/{day},{hour}:{minute}:{second}";
        public override int GetHashCode() => HashCode.Combine(year, month, day, hour, minute, second);
        public override bool Equals(object obj) => obj is TimeValue other && Equals(other);
        public bool Equals(TimeValue other)
            => year == other.year && month == other.month && day == other.day
            && hour == other.hour && minute == other.minute && second == other.second;

        public static TimeValue Parse(string str)
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

            return new TimeValue(year, month, day, hour, minute, second);
        }
    }
}