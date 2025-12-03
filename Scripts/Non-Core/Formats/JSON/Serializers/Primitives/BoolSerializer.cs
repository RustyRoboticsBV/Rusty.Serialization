using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class BoolSerializer : JsonSerializer<BoolNode>
    {
        /* Public properties. */
        public override string Tag => "bool";

        /* Public methods. */
        public override string Serialize(BoolNode node, ISerializerScheme scheme)
        {
            StringBuilder sb = new();
            sb.Append('{');
            AddItem(sb, "type", Tag, true, scheme.PrettyPrint, scheme.Tab);
            AddItem(sb, "value", node.Value ? "true" : "false", false, scheme.PrettyPrint, scheme.Tab);
            sb.Append('}');
            return sb.ToString();
        }

        public override BoolNode Parse(string serialized, ISerializerScheme scheme)
        {
            throw new NotImplementedException();
        }
    }
}