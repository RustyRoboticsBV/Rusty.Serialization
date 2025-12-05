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

        /* Private properties. */
        private static StringBuilder sb = new();

        /* Public methods. */
        public override string Serialize(StringNode node, ISerializerScheme scheme)
        {
            OpenCollection(sb, '{');
            AddItem(sb, Tag, node.Value, true, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb, '}', scheme.PrettyPrint);
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