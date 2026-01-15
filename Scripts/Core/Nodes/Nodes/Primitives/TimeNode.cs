using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A time serializer node.
    /// </summary>
    public class TimeNode : INode
    {
        /* Public properties */
        public ITreeElement Parent { get; set; }
        public bool Negative { get; set; }
        public ulong Year { get; set; }
        public ulong Month { get; set; }
        public ulong Day { get; set; }
        public ulong Hour { get; set; }
        public ulong Minute { get; set; }
        public ulong Second { get; set; }
        public ulong Millisecond { get; set; }
        public ulong Nanosecond { get; set; }

        /* Constructors. */
        public TimeNode() { }

        public TimeNode(bool negative, ulong year, ulong month,
            ulong day, ulong hour, ulong minute, ulong second,
            ulong millisecond, ulong nanosecond)
        {
            Negative = negative;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
            Nanosecond = nanosecond;
        }

        /* Conversion operators. */
        public static explicit operator TimeNode(ObjectNode obj)
        {
            TimeNode time = new TimeNode();

            for (int i = 0; i < obj.Count; i++)
            {
                INode value = obj.Members[i].Value;
                switch (obj.Members[i].Key)
                {
                    case "negative":
                    case "-":
                        if (value is BoolNode negative)
                            time.Negative = negative.Value;
                        break;
                    case "year":
                        if (value is IntNode year)
                            time.Year = ulong.Parse(year.Value);
                        break;
                    case "month":
                        if (value is IntNode month)
                            time.Month = ulong.Parse(month.Value);
                        break;
                    case "day":
                        if (value is IntNode day)
                            time.Day = ulong.Parse(day.Value);
                        break;
                    case "hour":
                        if (value is IntNode hour)
                            time.Hour = ulong.Parse(hour.Value);
                        break;
                    case "minute":
                        if (value is IntNode minute)
                            time.Minute = ulong.Parse(minute.Value);
                        break;
                    case "second":
                        if (value is IntNode second)
                            time.Second = ulong.Parse(second.Value);
                        break;
                    case "ms":
                    case "millisecond":
                        if (value is IntNode millisecond)
                            time.Millisecond = ulong.Parse(millisecond.Value);
                        break;
                    case "ns":
                    case "nanosecond":
                        if (value is IntNode nanosecond)
                            time.Millisecond = ulong.Parse(nanosecond.Value);
                        break;
                    default:
                        throw new ArgumentException("Cannot parse time component " + obj.Members[i].Key);
                }
            }

            return time;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"Time: {(Negative ? "-" : "")}{Year}/{Month}/{Day}, {Hour}:{Minute}:{Second}.{Millisecond}.{Nanosecond}";
        }

        public void Clear()
        {
            Parent = null;
            Negative = false;
            Year = 0;
            Month = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
            Second = 0;
            Millisecond = 0;
            Nanosecond = 0; 
        }
    }
}