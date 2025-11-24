using System;
using System.Globalization;
using System.Text.RegularExpressions;
using static Godot.Time;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A datetime serializer node.
/// </summary>
public readonly struct DatetimeNode : INode
{
    /// <summary>
    /// An internal representation of a date-time-timezone tuple.
    /// Don't use this directly outside of DatetimeNode.
    /// </summary>
    public struct Datetime
    {
        public ushort year;
        public byte month;
        public byte day;

        public byte hour;
        public byte minute;
        public byte second;

        public ushort millisecond;

        public sbyte timezoneHour;
        public byte timezoneMinute;

        public Datetime(ushort year, byte month, byte day, byte hour, byte minute, byte second, ushort millisecond,
            sbyte timezoneHour, byte timezoneMinute)
        {
            this.year = year;
            this.month = month;
            this.day = day;

            this.hour = hour;
            this.minute = minute;
            this.second = second;

            this.millisecond = millisecond;

            this.timezoneHour = timezoneHour;
            this.timezoneMinute = timezoneMinute;
        }

        public Datetime(DateTimeOffset value)
        {
            year = (ushort)value.Year;
            month = (byte)value.Month;
            day = (byte)value.Day;

            hour = (byte)value.Hour;
            minute = (byte)value.Minute;
            second = (byte)value.Second;

            millisecond = (ushort)value.Millisecond;

            timezoneHour = (sbyte)value.Offset.Hours;
            timezoneMinute = (byte)value.Offset.Minutes;
        }

        public static implicit operator Datetime(DateTime dt) => new(new(dt));
        public static implicit operator Datetime(DateTimeOffset dt) => new(dt);
        public static implicit operator DateTime(Datetime dt)
            => new(dt.year, dt.month, dt.day, dt.hour, dt.minute, dt.second, dt.millisecond);
        public static implicit operator DateTimeOffset(Datetime dt)
            => new((DateTime)dt, new TimeSpan(dt.timezoneHour, dt.timezoneMinute, 0));

        public override string ToString()
        {
            return $"{year.ToString("D4")}:{month.ToString("D2")}:{day.ToString("D2")}"
                + $" {hour.ToString("D2")}:{minute.ToString("D2")}:{second.ToString("D2")}:{millisecond.ToString("D3")}"
                + $" {(timezoneHour >= 0 ? "+" : "")}{timezoneHour.ToString("D2")}:{timezoneMinute.ToString("D2")}";
        }
    }

    /* Fields. */
    private readonly Datetime value;

    /* Public properties */
    public readonly Datetime Value => value;

    /* Constructors. */
    public DatetimeNode(Datetime value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return $"datetime: " + value;
    }

    public readonly string Serialize()
    {
        // Base date/time
        string date = value.year.ToString("D4") +
                          value.month.ToString("D2") +
                          value.day.ToString("D2");
        if (date == "00000000")
            date = "";

        string time = value.hour.ToString("D2") +
                          value.minute.ToString("D2") +
                          value.second.ToString("D2");
        if (time == "000000")
            time = "";

        // Milliseconds.
        string milliseconds = value.millisecond > 0 ? "_" + value.millisecond.ToString("D3") : "";

        // Timezone.
        string tzPart = "";
        if (value.timezoneHour != 0 && value.timezoneMinute != 0)
        {
            string sign = value.timezoneHour < 0 ? "-" : "+";
            int hours = Math.Abs(value.timezoneHour);
            int minutes = Math.Abs(value.timezoneMinute);
            tzPart = $"_{sign}{hours:D2}{minutes:D2}";
        }

        return $"{date}_{time}{milliseconds}{tzPart}";
    }

    public static DatetimeNode Parse(string text)
    {
        // Remove whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Enforce at least one underscore.
            if (!trimmed.Contains('_'))
                throw new ArgumentException("Missing underscore.");

            // Split into parts.
            string[] parts = trimmed.Split('_');

            // Interpret parts.
            const string dateFormat = "YYYYMMDD";
            const string timeFormat = "HHMMSS";
            const string msFormat = "FFF";
            const string timezoneFormat = "±HHMM";

            string date = null;
            string time = null;
            string ms = null;
            string timezone = null;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == dateFormat.Length)
                {
                    if (date == null)
                        date = parts[i];
                    else
                        throw new ArgumentException("Double date part.");
                }
                else if (parts[i].Length == timeFormat.Length)
                {
                    if (time == null)
                        time = parts[i];
                    else
                        throw new ArgumentException("Double time part.");
                }
                else if (parts[i].Length == msFormat.Length)
                {
                    if (ms == null)
                        ms = parts[i];
                    else
                        throw new ArgumentException("Double millisecond part.");
                }
                else if (parts[i].Length == timezoneFormat.Length)
                {
                    if (timezone == null)
                        timezone = parts[i];
                    else
                        throw new ArgumentException("Double timezone.");
                }
            }

            // Parse.
            Datetime dt = new();
            if (date != null)
            {
                dt.year = ushort.Parse(date.Substring(0, 4));
                dt.month = byte.Parse(date.Substring(4, 2));
                dt.day = byte.Parse(date.Substring(6, 2));
            }
            if (time != null)
            {
                dt.hour = byte.Parse(time.Substring(0, 2));
                dt.minute = byte.Parse(time.Substring(2, 2));
                dt.second = byte.Parse(time.Substring(4, 2));
            }
            if (ms != null)
                dt.millisecond = ushort.Parse(ms);
            if (timezone != null)
            {
                dt.timezoneHour = sbyte.Parse(timezone.Substring(0, 3));
                dt.timezoneMinute = byte.Parse(timezone.Substring(3, 2));
            }

            return new(dt);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as an integer:\n{ex.Message}");
        }
    }
}