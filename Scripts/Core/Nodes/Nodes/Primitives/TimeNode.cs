using System;
using System.Numerics;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A time serializer node.
    /// </summary>
    public class TimeNode : INode
    {
        /// <summary>
        /// An internal representation of a date-time-timezone tuple.
        /// Don't use this directly outside of DatetimeNode.
        /// </summary>
        public struct Timestamp
        {
            public bool negative;

            public BigInteger year;
            public BigInteger month;
            public BigInteger day;

            public BigInteger hour;
            public BigInteger minute;
            public BigInteger second;
            public BigInteger millisecond;

            public Timestamp(BigInteger year, BigInteger month, BigInteger day, BigInteger hour, BigInteger minute, BigInteger second, BigInteger millisecond)
            {
                negative = year < 0
                    || (year == 0 && month < 0)
                    || (year == 0 && month == 0 && day < 0)
                    || (year == 0 && month == 0 && day == 0 && hour < 0)
                    || (year == 0 && month == 0 && day == 0 && hour == 0 && minute < 0)
                    || (year == 0 && month == 0 && day == 0 && hour == 0 && minute == 0 && second < 0)
                    || (year == 0 && month == 0 && day == 0 && hour == 0 && minute == 0 && second == 0 && millisecond < 0);

                this.year = BigInteger.Abs(year);
                this.month = BigInteger.Abs(month);
                this.day = BigInteger.Abs(day);

                this.hour = BigInteger.Abs(hour);
                this.minute = BigInteger.Abs(minute);
                this.second = BigInteger.Abs(second);
                this.millisecond = BigInteger.Abs(millisecond);
            }

            public Timestamp(DateTime value) : this(
                value.Year, value.Month, value.Day,
                value.Hour, value.Minute, value.Second, value.Millisecond)
            { }

            public Timestamp(TimeSpan value) : this(
                0, 0, value.Days,
                value.Hours, value.Minutes, value.Seconds, value.Milliseconds)
            { }

            public override string ToString()
            {
                return $"{(negative ? "-" : "")}{year}/{month}/{day}, {hour}:{minute}:{second}.{millisecond}";
            }
        }

        /* Public properties */
        public INode Parent { get; set; }
        public Timestamp Value { get; set; }

        /* Constructors. */
        public TimeNode(Timestamp value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"timestamp: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = new();
        }
    }
}