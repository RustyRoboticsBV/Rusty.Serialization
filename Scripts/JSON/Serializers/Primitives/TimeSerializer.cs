using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON time serializer.
    /// </summary>
    public class TimeSerializer : Serializer<TimeNode>
    {
        /* Public methods. */
        public override string Serialize(TimeNode node, ISerializerScheme scheme)
        {
            // Build string.
            StringBuilder str = new();
            if (node.Negative)
                str.AppendLine("-");
            if (node.Year != 0)
                str.Append($"Y{node.Year}");
            if (node.Month != 0)
                str.Append($"M{node.Month}");
            if (node.Day != 0)
                str.Append($"D{node.Day}");
            if (node.Hour != 0)
                str.Append($"h{node.Hour}");
            if (node.Minute != 0)
                str.Append($"m{node.Minute}");
            if (node.Second != 0)
                str.Append($"s{node.Second}");
            if (node.Millisecond != 0)
                str.Append($"g{node.Millisecond}");
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

                // Interpret terms.
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
                                throw new ArgumentException("Duplicate g.");
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
                return new(negative, year.Value, month.Value, day.Value,
                    hour.Value, minute.Value, second.Value, millisecond.Value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a time:\n{ex.Message}");
            }
        }

        /* Private methods. */
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
}