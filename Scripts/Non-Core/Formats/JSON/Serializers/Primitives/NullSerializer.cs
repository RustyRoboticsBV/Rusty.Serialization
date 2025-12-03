using System;
using System.Text;
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
            StringBuilder sb = new();
            sb.Append('{');
            AddItem(sb, "type", "null", true, scheme.PrettyPrint, scheme.Tab);
            sb.Append('}');
            return sb.ToString();
        }

        public override NullNode Parse(string serialized, ISerializerScheme scheme)
        {
            throw new NotImplementedException();
        }
    }
}