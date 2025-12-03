using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class RealSerializer : JsonSerializer<RealNode>
    {
        /* Public properties. */
        public override string Tag => "real";

        /* Public methods. */
        public override string Serialize(RealNode node, ISerializerScheme scheme)
        {
            JsonPrimitive<PeterO.Numbers.EDecimal> json = new(Tag, node.Value);
            return NodeToText(json, scheme);
        }

        public override RealNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            // Deserialize.
            var json = TextToNode<JsonPrimitive<PeterO.Numbers.EDecimal>>(serialized);
            return new(json.value);
        }
    }
}