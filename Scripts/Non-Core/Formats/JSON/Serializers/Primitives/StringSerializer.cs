using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class StringSerializer : JsonSerializer<StringNode>
    {
        /* Public properties. */
        public override string Tag => "string";

        /* Public methods. */
        public override string Serialize(StringNode node, ISerializerScheme scheme)
        {
            StringBuilder sb = new();
            sb.Append('{');
            AddItem(sb, "type", Tag, true, scheme.PrettyPrint, scheme.Tab);
            AddItem(sb, "value", node.Value, true, scheme.PrettyPrint, scheme.Tab);
            sb.Append("\n}");
            return sb.ToString();
        }

        public override StringNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            // Deserialize.
            var json = TextToNode<JsonPrimitive<string>>(serialized);
            return new(json.value);
        }
    }
}