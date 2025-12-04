using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Text;

namespace Rusty.Serialization.Serializers.JSON
{
    public class ListSerializer : JsonSerializer<ListNode>
    {
        /* Public properties. */
        public override string Tag => "list";

        /* Public methods. */
        public override string Serialize(ListNode node, ISerializerScheme scheme)
        {
            StringBuilder sb = new();
            sb.Append('[');
            for (int i = 0; i < node.Elements.Length; i++)
            {
                string value = scheme.Serialize(node.Elements[i]);
                AddItem(sb, "", value, false, scheme.PrettyPrint, scheme.Tab);
            }
            sb.Append("\n]");

            StringBuilder sb2 = new();
            sb2.Append('{');
            AddItem(sb2, "type", Tag, true, scheme.PrettyPrint, scheme.Tab);
            AddItem(sb2, "value", sb.ToString(), false, scheme.PrettyPrint, scheme.Tab);
            sb2.Append("\n}");
            return sb2.ToString();
        }

        public override ListNode Parse(string serialized, ISerializerScheme scheme)
        {
            // TODO: implement
            return new();
        }
    }
}