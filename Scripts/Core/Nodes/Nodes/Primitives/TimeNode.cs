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
        public IntString Year { get; set; }
        public UnsignedIntString Month { get; set; }
        public UnsignedIntString Day { get; set; }
        public UnsignedIntString Hour { get; set; }
        public UnsignedIntString Minute { get; set; }
        public UnsignedRealString Second { get; set; }

        /* Constructors. */
        public TimeNode() { }

        public TimeNode(IntString year, UnsignedIntString month, UnsignedIntString day,
            UnsignedIntString hour, UnsignedIntString minute, UnsignedRealString second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
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
                    case "year":
                        if (value is IntNode year)
                            time.Year = year.Value;
                        else
                            throw new FormatException("Invalid year node.");
                        break;
                    case "month":
                        if (value is IntNode month)
                            time.Month = month.Value;
                        else
                            throw new FormatException("Invalid month node.");
                        break;
                    case "day":
                        if (value is IntNode day)
                            time.Day = day.Value;
                        else
                            throw new FormatException("Invalid day node.");
                        break;
                    case "hour":
                        if (value is IntNode hour)
                            time.Hour = hour.Value;
                        else
                            throw new FormatException("Invalid hour node.");
                        break;
                    case "minute":
                        if (value is IntNode minute)
                            time.Minute = minute.Value;
                        else
                            throw new FormatException("Invalid minute node.");
                        break;
                    case "second":
                        if (value is IntNode secondi)
                            time.Second = secondi.Value;
                        else if (value is FloatNode secondf)
                            time.Second = secondf.Value;
                        else
                            throw new FormatException("Invalid second node.");
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
            return $"Time: {Year}/{Month}/{Day}, {Hour}:{Minute}:{Second}";
        }

        public void Clear()
        {
            Parent = null;
            Year = 0;
            Month = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
            Second = 0.0;
        }
    }
}