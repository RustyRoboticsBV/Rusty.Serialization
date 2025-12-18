using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;
using DateTime = System.DateTime;

namespace Rusty.Serialization.Dotnet
{
    /// <summary>
    /// A date/time converter.
    /// </summary>
    public sealed class DateTimeConverter : Converter<DateTime, TimeNode>
    {
        /* Protected methods. */
        protected override TimeNode CreateNode(DateTime obj, CreateNodeContext context)
            => new(IsNegative(ref obj), (ulong)obj.Year, (ulong)obj.Month, (ulong)obj.Day,
                (ulong)obj.Hour, (ulong)obj.Minute, (ulong)obj.Second,
                (ulong)obj.Millisecond);

        protected override DateTime CreateObject(TimeNode node, CreateObjectContext context)
        {
            int year = (int)node.Year;
            int month = (int)node.Month;
            int day = (int)node.Day;
            int hour = (int)node.Hour;
            int minute = (int)node.Minute;
            int second = (int)node.Second;
            int millisecond = (int)node.Millisecond;

            if (node.Negative)
            {
                if (year != 0)
                    year = -year;
                else if (month != 0)
                    month = -month;
                else if (day != 0)
                    day = -day;
                else if (hour != 0)
                    hour = -hour;
                else if (minute != 0)
                    minute = -minute;
                else if (second != 0)
                    second = -second;
                else if (millisecond != 0)
                    millisecond = -millisecond;
            }

            return new(year, month, day, hour, minute, second, millisecond);
        }

        /* Private methods. */
        private static bool IsNegative(ref DateTime obj)
        {
            return obj.Year < 0
                || obj.Year == 0 && obj.Month < 0
                || obj.Year == 0 && obj.Month == 0 && obj.Day < 0
                || obj.Year == 0 && obj.Month == 0 && obj.Day == 0 && obj.Hour < 0
                || obj.Year == 0 && obj.Month == 0 && obj.Day == 0 && obj.Hour == 0 && obj.Minute < 0
                || obj.Year == 0 && obj.Month == 0 && obj.Day == 0 && obj.Hour == 0 && obj.Minute == 0 && obj.Second < 0
                || obj.Year == 0 && obj.Month == 0 && obj.Day == 0 && obj.Hour == 0 && obj.Minute == 0 && obj.Second == 0 && obj.Millisecond < 0;
        }
    }
}