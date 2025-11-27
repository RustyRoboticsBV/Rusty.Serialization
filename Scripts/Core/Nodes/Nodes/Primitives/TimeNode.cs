using System;
using System.Numerics;
using System.Text;

namespace Rusty.Serialization.Nodes
{
    /// <summary>
    /// A time serializer node.
    /// </summary>
    public readonly struct TimeNode : INode
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

        /* Fields. */
        private readonly Timestamp value;

        /* Public properties */
        public readonly Timestamp Value => value;

        /* Constructors. */
        public TimeNode(Timestamp value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return $"timestamp: " + value;
        }

        public readonly string Serialize()
        {
            // Build string.
            StringBuilder str = new();
            if (value.negative)
                str.AppendLine("-");
            if (value.year != 0)
                str.Append($"Y{value.year}");
            if (value.month != 0)
                str.Append($"M{value.month}");
            if (value.day != 0)
                str.Append($"D{value.day}");
            if (value.hour != 0)
                str.Append($"h{value.hour}");
            if (value.minute != 0)
                str.Append($"m{value.minute}");
            if (value.second != 0)
                str.Append($"s{value.second}");
            if (value.millisecond != 0)
                str.Append($"f{value.millisecond}");
            string serialized = str.ToString();

            // Handle all zeros.
            if (serialized.Length == 0)
                return "Y0";

            // Otherwise, return serialized value.
            return serialized;
        }

        public static TimeNode Parse(string text)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Check negative sign.
                bool negative = trimmed.StartsWith('-');

                // Interpret.
                BigInteger? year = null;
                BigInteger? month = null;
                BigInteger? day = null;
                BigInteger? hour = null;
                BigInteger? minute = null;
                BigInteger? second = null;
                BigInteger? millisecond = null;

                for (int i = negative ? 1 : 0; i < trimmed.Length; i++)
                {
                    switch (trimmed[i])
                    {
                        case 'Y':
                            if (year == null)
                                year = Parse(trimmed, ref i);
                            else
                                throw new ArgumentException("Duplicate Y.");
                            break;
                        case 'M':
                            if (month == null)
                                month = Parse(trimmed, ref i);
                            else
                                throw new ArgumentException("Duplicate M.");
                            break;
                        case 'D':
                            if (day == null)
                                day = Parse(trimmed, ref i);
                            else
                                throw new ArgumentException("Duplicate D.");
                            break;
                        case 'h':
                            if (hour == null)
                                hour = Parse(trimmed, ref i);
                            else
                                throw new ArgumentException("Duplicate h.");
                            break;
                        case 'm':
                            if (minute == null)
                                minute = Parse(trimmed, ref i);
                            else
                                throw new ArgumentException("Duplicate m.");
                            break;
                        case 's':
                            if (second == null)
                                second = Parse(trimmed, ref i);
                            else
                                throw new ArgumentException("Duplicate s.");
                            break;
                        case 'f':
                            if (millisecond == null)
                                millisecond = Parse(trimmed, ref i);
                            else
                                throw new ArgumentException("Duplicate f.");
                            break;
                        default:
                            throw new ArgumentException($"Invalid term '{trimmed[i]}'.");
                    }
                }

                // Default missing values to 0.
                year = year ?? 0;
                month = month ?? 0;
                day = day ?? 0;
                hour = hour ?? 0;
                minute = minute ?? 0;
                second = second ?? 0;
                millisecond = millisecond ?? 0;

                // Apply negative sign.
                if (negative)
                {
                    if (year > 0)
                        year = -year;
                    else if (month > 0)
                        month = -month;
                    else if (day > 0)
                        day = -day;
                    else if (hour > 0)
                        hour = -hour;
                    else if (minute > 0)
                        minute = -minute;
                    else if (second > 0)
                        second = -second;
                    else if (millisecond > 0)
                        millisecond = -millisecond;
                }

                // Create node.
                return new(new(year.Value, month.Value, day.Value,
                    hour.Value, minute.Value, second.Value, millisecond.Value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as an integer:\n{ex.Message}");
            }
        }

        private static BigInteger Parse(string str, ref int index)
        {
            int i;
            for (i = index + 1; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > '9')
                    break;
            }
            if (i == index + 1)
                throw new Exception($"Empty term '{str[index]}'.");
            BigInteger value = BigInteger.Parse(str.Substring(index + 1, i - (index + 1)));
            index = i - 1;
            return value;
        }
    }
}