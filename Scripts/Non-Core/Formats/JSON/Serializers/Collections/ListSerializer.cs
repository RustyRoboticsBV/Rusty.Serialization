using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class ListSerializer : JsonSerializer<ListNode>
    {
        /* Public properties. */
        public override string Tag => "list";

        /* Public methods. */
        public override string Serialize(ListNode node, ISerializerScheme scheme)
        {
            JsonNode[] elements = new JsonNode[node.Elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = new JsonSnippet(scheme.Serialize(node.Elements[i]));
            }
            JsonArray json = new(Tag, elements);

            return NodeToText(json, scheme);
        }

        public override ListNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return new();
        }
    }
}