using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class BoolSerializer : JsonSerializer<BoolNode>
    {
        /* Public properties. */
        public override string Tag => "bool";

        /* Public methods. */
        public override string Serialize(BoolNode node, ISerializerScheme scheme)
        {
            JsonPrimitive<bool> json = new(Tag, node.Value);
            return NodeToText(json, scheme);
        }

        public override BoolNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            // Deserialize.
            var json = TextToNode<JsonPrimitive<bool>>(serialized);
            return new(json.value);
        }
    }
}