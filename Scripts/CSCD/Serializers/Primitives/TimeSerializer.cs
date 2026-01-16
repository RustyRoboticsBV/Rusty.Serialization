using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
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
            if (!(node.Year.IsOne && node.Month.IsOne && node.Day.IsOne))
                str.Append($"{node.Year}-{node.Month}-{node.Day}");
            if (!(node.Hour.IsZero && node.Minute.IsZero && node.Second.IsZero))
            {
                if (str.Length > 0)
                    str.Append("_");
                str.Append($"{node.Hour}:{node.Minute}:{(node.Second.IsIntegral ? (UnsignedIntString)node.Second : node.Second)}");
            }

            // Return serialized value.
            return '@' + str.ToString() + ';';
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

                // Enforce ampersand prefix.
                if (!trimmed.StartsWith('@'))
                    throw new FormatException("Missing @ character.");
                if (!trimmed.EndsWith(';'))
                    throw new FormatException("Missing ; character.");

                // Split on underscore.
                string[] dateTimeSplit = trimmed.Substring(1, trimmed.Length - 2).Split('_');

                if (dateTimeSplit.Length == 2)
                {
                    ParseDate(dateTimeSplit[0], out IntString year, out UnsignedIntString month, out UnsignedIntString day);
                    ParseTime(dateTimeSplit[1], out UnsignedIntString hour, out UnsignedIntString minute, out RealString second);
                    return new TimeNode(year, month, day, hour, minute, second);
                }
                else if (dateTimeSplit.Length == 1)
                {
                    if (dateTimeSplit[0].Contains('-'))
                    {
                        ParseDate(dateTimeSplit[0], out IntString year, out UnsignedIntString month, out UnsignedIntString day);
                        return new TimeNode(year, month, day, 0, 0, 0.0);
                    }
                    else if (dateTimeSplit[0].Contains(':'))
                    {
                        ParseTime(dateTimeSplit[0], out UnsignedIntString hour, out UnsignedIntString minute, out RealString second);
                        return new TimeNode(1, 1, 1, hour, minute, second);
                    }
                }
                throw new FormatException("Bad literal.");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a time:\n{ex.Message}");
            }
        }

        /* Private methods. */
        private static void ParseDate(string str, out IntString year, out UnsignedIntString month, out UnsignedIntString day)
        {
            bool negativeYear = str.StartsWith('-');
            if (negativeYear)
                str = str.Substring(1);

            string[] terms = str.Split('-');
            if (terms.Length != 3)
                throw new FormatException("Dates must have 3 terms.");
            year = negativeYear ? '-' + terms[0] : terms[0];
            month = terms[1];
            day = terms[2];
        }

        private static void ParseTime(string str, out UnsignedIntString hour, out UnsignedIntString minute, out RealString second)
        {
            string[] terms = str.Split(':');
            if (terms.Length != 3)
                throw new FormatException("Times must have 3 terms.");
            hour = terms[0];
            minute = terms[1];
            second = terms[2];
        }
    }
}