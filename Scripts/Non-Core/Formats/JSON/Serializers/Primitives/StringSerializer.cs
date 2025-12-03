using System;
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
            JsonPrimitive<string> json = new(Tag, node.Value);
            return NodeToText(json, scheme);
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