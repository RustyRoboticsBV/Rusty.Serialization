using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class NullSerializer : JsonSerializer<NullNode>
    {
        /* Public properties. */
        public override string Tag => "null";

        /* Public methods. */
        public override string Serialize(NullNode node, ISerializerScheme scheme)
        {
            JsonPrimitive<string> json = new(Tag, "null");
            return NodeToText(json, scheme);
        }

        public override NullNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            return new();
        }
    }
}