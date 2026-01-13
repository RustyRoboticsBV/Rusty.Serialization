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
                return "\"\"";

            // Otherwise, return serialized value.
            return '"' + serialized + '"';
        }

        public override TimeNode Parse(string text, ISerializerScheme scheme)
        {
            return (TimeNode)scheme.ParseAsNode(text);
        }
    }
}