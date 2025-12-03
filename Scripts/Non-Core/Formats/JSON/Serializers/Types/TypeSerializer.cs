using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class TypeSerializer : JsonSerializer<TypeNode>
    {
        /* Public properties. */
        public override string Tag => "type";

        /* Public methods. */
        public override string Serialize(TypeNode node, ISerializerScheme scheme)
        {
            JsonType json = new(Tag, node.Name, new JsonSnippet(scheme.Serialize(node.Value)));
            return NodeToText(json, scheme);
        }

        public override TypeNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return new();
        }
    }
}