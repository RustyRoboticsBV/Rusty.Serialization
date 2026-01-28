using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A time node value.
    /// </summary>
    public readonly struct TimeValue : IEquatable<TimeValue>
    {
        // TODO: placeholder; remove.
        public struct FloatValue
        {
            public static bool operator ==(FloatValue a, FloatValue b) => true;
            public static bool operator !=(FloatValue a, FloatValue b) => false;
        }

        /* Fields */
        public readonly IntValue year;
        public readonly byte month;
        public readonly byte day;
        public readonly byte hour;
        public readonly byte minute;
        public readonly FloatValue second;

        /* Constructors */
        public TimeValue(IntValue year, byte month, byte day, byte hour, byte minute, FloatValue second)
        {
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