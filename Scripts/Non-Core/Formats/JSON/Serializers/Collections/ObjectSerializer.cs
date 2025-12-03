using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class ObjectSerializer : JsonSerializer<ObjectNode>
    {
        /* Public properties. */
        public override string Tag => "object";

        /* Public methods. */
        public override string Serialize(ObjectNode node, ISerializerScheme scheme)
        {
            JsonNode[] elements = new JsonNode[node.Members.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                JsonKeyValuePair<string, JsonNode> pair = new(
                    node.Members[i].Key,
                    new JsonSnippet(scheme.Serialize(node.Members[i].Value))
                );
                elements[i] = pair;
            }
            JsonArray json = new(Tag, elements);

            return NodeToText(json, scheme);
        }

        public override ObjectNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return new();
        }
    }
}