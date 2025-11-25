using System;
using System.Text;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A datetime serializer node.
/// </summary>
public readonly struct TimestampNode : INode
{
    /// <summary>
    /// An internal representation of a date-time-timezone tuple.
    /// Don't use this directly outside of DatetimeNode.
    /// </summary>
    public struct Timestamp
    {
        public bool negative;
        public ulong year;
        public ulong month;
        public ulong day;

        public ulong hour;
        public ulong minute;
        public ulong second;
        public ulong millisecond;

        public Timestamp(bool negative, ulong year, ulong month, ulong day, ulong hour, ulong minute, ulong second, ulong millisecond)
        {
            this.negative = negative;
            this.year = year;
            this.month = month;
            this.day = day;

            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.millisecond = millisecond;
        }

        public Timestamp(DateTime value)
        {
            negative = value.Year < 0;
            year = (ulong)Math.Abs(value.Year);
            month = (ulong)value.Month;
            day = (ulong)value.Day;

            hour = (ulong)value.Hour;
            minute = (ulong)value.Minute;
            second = (ulong)value.Second;
            millisecond = (ulong)value.Millisecond;
        }

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
    public TimestampNode(Timestamp value)
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

    public static TimestampNode Parse(string text)
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
            ulong? year = null;
            ulong? month = null;
            ulong? day = null;
            ulong? hour = null;
            ulong? minute = null;
            ulong? second = null;
            ulong? millisecond = null;

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

            // Create node.
            return new(new(negative, (ulong)year, (ulong)month, (ulong)day,
                (ulong)hour, (ulong)minute, (ulong)second, (ulong)millisecond));
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as an integer:\n{ex.Message}");
        }
    }

    private static ulong Parse(string str, ref int index)
    {
        int i;
        for (i = index + 1; i < str.Length; i++)
        {
            if (str[i] < '0' || str[i] > '9')
                break;
        }
        if (i == index + 1)
            throw new Exception($"Empty term '{str[index]}'.");
        ulong value = ulong.Parse(str.Substring(index + 1, i - (index + 1)));
        index = i - 1;
        return value;
    }
}