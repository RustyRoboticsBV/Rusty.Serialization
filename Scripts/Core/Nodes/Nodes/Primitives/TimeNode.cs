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

        /* Constructors. */
        public TimeNode(bool negative, ulong year, ulong month, ulong day, ulong hour, ulong minute, ulong second, ulong millisecond)
        {
            Negative = negative;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"Time: {(Negative ? "-" : "")}{Year}/{Month}/{Day}, {Hour}:{Minute}:{Second}.{Millisecond}";
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
        }
    }
}