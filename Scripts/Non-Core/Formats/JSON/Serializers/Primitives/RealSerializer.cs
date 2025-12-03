using System;
using System.Text;
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
            StringBuilder sb = new();
            sb.Append('{');
            AddItem(sb, "type", Tag, true, scheme.PrettyPrint, scheme.Tab);
            AddItem(sb, "value", node.Value.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            sb.Append('}');
            return sb.ToString();
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