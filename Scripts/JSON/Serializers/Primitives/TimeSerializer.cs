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
            StringBuilder str = new StringBuilder();

            Serialize(str, "negative", node.Negative, scheme);
            Serialize(str, "year", node.Year, scheme);
            Serialize(str, "month", node.Month, scheme);
            Serialize(str, "day", node.Day, scheme);
            Serialize(str, "hour", node.Hour, scheme);
            Serialize(str, "minute", node.Minute, scheme);
            Serialize(str, "second", node.Second, scheme);
            Serialize(str, "ms", node.Millisecond, scheme);
            Serialize(str, "ns", node.Nanosecond, scheme);

            // Otherwise, return serialized value.
            return '{' + str.ToString() + "\n}";
        }

        public override TimeNode Parse(string text, ISerializerScheme scheme)
        {
            return (TimeNode)scheme.ParseAsNode(text);
        }

        /* Private methods. */
        private void Serialize(StringBuilder str, string key, bool value, ISerializerScheme scheme)
        {
            Serialize(str, key, $"{(value ? "true" : "false")}", scheme);
        }

        private void Serialize(StringBuilder str, string key, ulong value, ISerializerScheme scheme)
        {
            Serialize(str, key, $"{value}", scheme);
        }

        private void Serialize(StringBuilder str, string key, string value, ISerializerScheme scheme)
        {
            if (str.Length != 0)
                str.Append(',');

            if (scheme.PrettyPrint)
            {
                str.Append('\n');
                str.Append(scheme.Tab);
            }

            str.Append('"');
            str.Append(key);
            str.Append('"');
            
            if (scheme.PrettyPrint)
                str.Append(' ');
            str.Append(':');
            if (scheme.PrettyPrint)
                str.Append(' ');

            str.Append($"{value}");
        }
    }
}