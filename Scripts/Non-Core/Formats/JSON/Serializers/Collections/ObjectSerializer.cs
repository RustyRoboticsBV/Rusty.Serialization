using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Text;

namespace Rusty.Serialization.Serializers.JSON
{
    public class ObjectSerializer : JsonSerializer<ObjectNode>
    {
        /* Public properties. */
        public override string Tag => "object";

        /* Public methods. */
        public override string Serialize(ObjectNode node, ISerializerScheme scheme)
        {
            StringBuilder sb = new();
            sb.Append('[');
            for (int i = 0; i < node.Members.Length; i++)
            {
                string key = node.Members[i].Key;
                string value = scheme.Serialize(node.Members[i].Value);

                StringBuilder sb2 = new();
                sb2.Append('{');
                AddItem(sb2, "id", value, false, scheme.PrettyPrint, scheme.Tab);
                sb2.Append('}');
                sb.Append(sb2);
            }
            sb.Append(']');

            StringBuilder sb3 = new();
            sb3.Append('{');
            AddItem(sb3, "type", Tag, true, scheme.PrettyPrint, scheme.Tab);
            AddItem(sb3, "value", sb.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            sb3.Append('}');
            return sb3.ToString();
        }

        public override ObjectNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return new();
        }
    }
}