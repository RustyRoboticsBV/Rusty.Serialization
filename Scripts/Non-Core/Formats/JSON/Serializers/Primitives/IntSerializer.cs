using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class IntSerializer : JsonSerializer<IntNode>
    {
        /* Public properties. */
        public override string Tag => "int";

        /* Private properties. */
        private static StringBuilder sb = new();

        /* Public methods. */
        public override string Serialize(IntNode node, ISerializerScheme scheme)
        {
            OpenCollection(sb, '{');
            AddItem(sb, Tag, node.Value.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb, '}', scheme.PrettyPrint);
            return sb.ToString();
        }

        public override IntNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            // Deserialize.
            var json = TextToNode<JsonPrimitive<decimal>>(serialized);
            return new(json.value);
        }
    }
}