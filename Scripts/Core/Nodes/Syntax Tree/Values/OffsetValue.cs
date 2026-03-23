using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An offset node value.
    /// </summary>
    public readonly struct OffsetValue : IEquatable<OffsetValue>
    {
        /* Fields */
        public readonly bool sign;
        public readonly IntValue hours;
        public readonly IntValue minutes;

        /* Public properties. */
        public static OffsetValue UTC0 => new OffsetValue(true, 0, 0);

        /* Constructors */
        public OffsetValue(bool sign, IntValue hours, IntValue minutes)
        {
            if (hours < 0)
                throw new ArgumentException("Hours may not be negative.");
            if (minutes < 0)
                throw new ArgumentException("Minutes may not be negative.");

            this.sign = sign;
            this.hours = hours;
            this.minutes = minutes;
        }

        /* Public methods */
        public override string ToString() => hours == 0 && minutes == 0 ? "Z" : $"{(sign ? "+" : "-")}{hours:00}:{minutes:00}";
        public override int GetHashCode() => HashCode.Combine(sign, hours, minutes);
        public override bool Equals(object obj) => obj is OffsetValue other && Equals(other);
        public bool Equals(OffsetValue other) => sign == other.sign && hours == other.hours && minutes == other.minutes;

        public static OffsetValue Parse(string str) => Parse(str.AsSpan());
        public static OffsetValue Parse(ReadOnlySpan<char> span)
        {
            // UTC-0.
            if (span.Length == 0 || span.SequenceEqual("Z"))
                return new OffsetValue(true, 0, 0);

            // Sign.
            if (span[0] != '+' && span[0] != '-')
                throw new FormatException("Must start with + or - sign.");
            bool sign = span[0] == '+';
            span = span.Slice(1);

            // Separator.
            int separator = span.IndexOf(':');

            // Hours.
            IntValue hours;
            if (separator >= 0)
                hours = IntValue.Parse(span.Slice(0, separator));
            else
                hours = IntValue.Parse(span.Slice(0));

            // Minutes.
            IntValue minutes;
            if (separator >= 0)
                minutes = IntValue.Parse(span.Slice(separator + 1));
            else
                minutes = 0;


            return new OffsetValue(sign, hours, minutes);
        }
    }
}