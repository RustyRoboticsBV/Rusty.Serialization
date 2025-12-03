using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class ColorSerializer : JsonSerializer<ColorNode>
    {
        /* Public properties. */
        public override string Tag => "color";

        /* Public methods. */
        public override string Serialize(ColorNode node, ISerializerScheme scheme)
        {
            JsonPrimitive<byte> json = new(Tag, node.R);
            return NodeToText(json, scheme);
        }

        public override ColorNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            // Deserialize.
            var json = TextToNode<JsonPrimitive<byte>>(serialized);
            return new(json.value, 0, 0, 0);
        }
    }
}