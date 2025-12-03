using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class CharSerializer : JsonSerializer<CharNode>
    {
        /* Public properties. */
        public override string Tag => "char";

        /* Public methods. */
        public override string Serialize(CharNode node, ISerializerScheme scheme)
        {
            JsonPrimitive<string> json = new(Tag, ((char)node.Value).ToString());
            return NodeToText(json, scheme);
        }

        public override CharNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            // Deserialize.
            var json = TextToNode<JsonPrimitive<string>>(serialized);
            return new(json.value[0]);
        }
    }
}