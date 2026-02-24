using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A duration node value.
    /// </summary>
    public readonly struct DurationValue : IEquatable<DurationValue>
    {
        /* Fields */
        public readonly BoolValue negative;
        public readonly IntValue days;
        public readonly IntValue hours;
        public readonly IntValue minutes;
        public readonly FloatValue seconds;

        /* Constructors */
        public DurationValue(BoolValue negative, IntValue days, IntValue hours, IntValue minutes, FloatValue seconds)
        {
            if (days.IsNegative)
                throw new ArgumentException("Days may not be negative.");
            if (hours.IsNegative)
                throw new ArgumentException("Hours may not be negative.");
            if (minutes.IsNegative)
                throw new ArgumentException("Minutes may not be negative.");
            if (seconds.negative)
                throw new ArgumentException("Second may not be negative.");

            this.negative = negative;
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
        }

        /* Public methods */
        public override string ToString() => $"{(negative ? "-" : "")}{days}d{hours}h{minutes}m{seconds}s";
        public override int GetHashCode() => HashCode.Combine(negative, days, hours, minutes, seconds);
        public override bool Equals(object obj) => obj is DurationValue other && Equals(other);
        public bool Equals(DurationValue other) => negative == other.negative && days == other.days
            && hours == other.hours && minutes == other.minutes && seconds == other.seconds;

        public static DurationValue Parse(ReadOnlySpan<char> str)
        {
            if (str.Length == 0)
                throw new ArgumentException("Input string is null or empty.", nameof(str));

            str = str.Trim();
            int length = str.Length;
            int index = 0;
            bool negative = false;

            // Negative sign.
            if (str[index] == '-')
            {
                negative = true;
                index++;
                if (index >= length)
                    throw new FormatException("Duration cannot be only a minus sign.");
            }

            // Parse units.
            IntValue days = 0, hours = 0, minutes = 0;
            FloatValue seconds = 0.0;
            char? lastUnit = null;

            while (index < length)
            {
                // Parse number.
                int start = index;
                bool hasDecimal = false;
                while (index < length && (char.IsDigit(str[index]) || (!hasDecimal && str[index] == '.')))
                {
                    if (str[index] == '.')
                        hasDecimal = true;
                    index++;
                }

                if (start == index)
                    throw new FormatException($"Expected number at position {index}.");

                ReadOnlySpan<char> numberStr = str.Slice(start, index - start);

                // Get unit.
                if (index >= length)
                    throw new FormatException("Expected d, h, m or s unit after number.");

                char unit = str[index];
                index++;

                // Enforce canonical order.
                if (lastUnit.HasValue && "dhms".IndexOf(unit) < "dhms".IndexOf(lastUnit.Value))
                    throw new FormatException($"Duration units out of order: {unit} after {lastUnit}.");

                lastUnit = unit;

                switch (unit)
                {
                    case 'd':
                        days = IntValue.Parse(numberStr);
                        break;
                    case 'h':
                        hours = IntValue.Parse(numberStr);
                        break;
                    case 'm':
                        minutes = IntValue.Parse(numberStr);
                        break;
                    case 's':
                        seconds = FloatValue.Parse(numberStr);
                        break;
                    default:
                        throw new FormatException($"Unknown duration unit: {unit}");
                }
            }

            // Create value.
            return new DurationValue(negative, days, hours, minutes, seconds);
        }
    }
}