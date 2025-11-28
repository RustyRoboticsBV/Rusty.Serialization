using System;
using System.Numerics;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD time serializer.
    /// </summary>
    public class TimeSerializer : Serializer<TimeNode>
    {
        /* Public methods. */
        public override string Serialize(TimeNode node, ISerializerScheme scheme)
        {
            // Build string.
            StringBuilder str = new();
            if (node.Value.negative)
                str.AppendLine("-");
            if (node.Value.year != 0)
                str.Append($"Y{node.Value.year}");
            if (node.Value.month != 0)
                str.Append($"M{node.Value.month}");
            if (node.Value.day != 0)
                str.Append($"D{node.Value.day}");
            if (node.Value.hour != 0)
                str.Append($"h{node.Value.hour}");
            if (node.Value.minute != 0)
                str.Append($"m{node.Value.minute}");
            if (node.Value.second != 0)
                str.Append($"s{node.Value.second}");
            if (node.Value.millisecond != 0)
                str.Append($"f{node.Value.millisecond}");
            string serialized = str.ToString();

            // Handle all zeros.
            if (serialized.Length == 0)
                return "Y0";

            // Otherwise, return serialized value.
            return serialized;
        }

        public override TimeNode Parse(string text, ISerializerScheme scheme)
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

        /* Private methods. */
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