using System;
using System.Text;
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
            StringBuilder sb = new();
            sb.Append('{');
            AddItem(sb, "type", Tag, true, scheme.PrettyPrint, scheme.Tab);
            AddItem(sb, "value", node.Value <= char.MaxValue ? node.Value.ToString() : throw new Exception(), false, scheme.PrettyPrint, scheme.Tab);
            sb.Append("\n}");
            return sb.ToString();
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