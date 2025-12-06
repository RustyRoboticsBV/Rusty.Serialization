using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class ListSerializer : JsonSerializer<ListNode>
    {
        /* Public properties. */
        public override string Tag => "list";

        /* Private properties. */
        private static StringBuilder sb = new();
        private static StringBuilder sb2 = new();

        /* Public methods. */
        public override string Serialize(ListNode node, ISerializerScheme scheme)
        {
            // Serialize elements.
            OpenCollection(sb, '[');
            for (int i = 0; i < node.Elements.Length; i++)
            {
                // Serialize element.
                string value = scheme.Serialize(node.Elements[i]);
                AddItem(sb, "", value, false, scheme.PrettyPrint, scheme.Tab);
            }
            CloseCollection(sb, ']', scheme.PrettyPrint);

            // Serialize main container.
            OpenCollection(sb2, '{');
            AddItem(sb2, Tag, sb.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb2, '}', scheme.PrettyPrint);
            return sb2.ToString();
        }

        public override ListNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return null;
        }
    }
}