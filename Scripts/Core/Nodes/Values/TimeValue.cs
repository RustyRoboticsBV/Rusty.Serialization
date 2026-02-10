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
        public override string ToString() => $"{year}-{month}-{day}, {hour}:{minute}:{second}";
        public override int GetHashCode() => HashCode.Combine(year, month, day, hour, minute, second);
        public override bool Equals(object obj) => obj is TimeValue other && Equals(other);
        public bool Equals(TimeValue other)
            => year == other.year && month == other.month && day == other.day
            && hour == other.hour && minute == other.minute && second == other.second;
    }
}