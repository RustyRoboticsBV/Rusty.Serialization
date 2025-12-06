using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class DictSerializer : JsonSerializer<DictNode>
    {
        /* Public properties. */
        public override string Tag => "dict";

        /* Private properties. */
        private static StringBuilder sb = new();
        private static StringBuilder sb2 = new();
        private static StringBuilder sb3 = new();

        /* Public methods. */
        public override string Serialize(DictNode node, ISerializerScheme scheme)
        {
            // Serialize members.
            OpenCollection(sb, '[');
            for (int i = 0; i < node.Pairs.Length; i++)
            {
                // Serialize key-value pair.
                string key = scheme.Serialize(node.Pairs[i].Key);
                string value = scheme.Serialize(node.Pairs[i].Value);

                OpenCollection(sb, '{');
                AddItem(sb2, "key", key, true, scheme.PrettyPrint, scheme.Tab);
                AddItem(sb2, "value", value, false, scheme.PrettyPrint, scheme.Tab);
                CloseCollection(sb2, '}', scheme.PrettyPrint);

                // Add to list.
                AddItem(sb, "", sb.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            }
            CloseCollection(sb, ']', scheme.PrettyPrint);

            // Serialize main container.
            OpenCollection(sb3, '{');
            AddItem(sb3, Tag, sb.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb3, '}', scheme.PrettyPrint);
            return sb3.ToString();
        }

        public override DictNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return null;
        }
    }
}