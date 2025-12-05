using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class ObjectSerializer : JsonSerializer<ObjectNode>
    {
        /* Public properties. */
        public override string Tag => "obj";

        /* Private properties. */
        private static StringBuilder sb = new();
        private static StringBuilder sb2 = new();
        private static StringBuilder sb3 = new();

        /* Public methods. */
        public override string Serialize(ObjectNode node, ISerializerScheme scheme)
        {
            // Serialize members.
            OpenCollection(sb, '[');
            for (int i = 0; i < node.Members.Length; i++)
            {
                // Serialize key-value pair.
                string key = node.Members[i].Key;
                string value = scheme.Serialize(node.Members[i].Value);

                OpenCollection(sb2, '{');
                AddItem(sb2, "id", key, true, scheme.PrettyPrint, scheme.Tab);
                AddItem(sb2, "value", value, false, scheme.PrettyPrint, scheme.Tab);
                CloseCollection(sb2, '}', scheme.PrettyPrint);

                // Add to list.
                AddItem(sb, "", sb2.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            }
            CloseCollection(sb, ']', scheme.PrettyPrint);

            // Serialize main container.
            OpenCollection(sb3, '{');
            AddItem(sb3, Tag, sb.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb3, '}', scheme.PrettyPrint);
            return sb3.ToString();
        }

        public override ObjectNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return new();
        }
    }
}