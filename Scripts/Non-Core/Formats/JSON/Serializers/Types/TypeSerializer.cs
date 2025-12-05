using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;
using System;
using System.Text;

namespace Rusty.Serialization.Serializers.JSON
{
    public class TypeSerializer : JsonSerializer<TypeNode>
    {
        /* Public properties. */
        public override string Tag => "type";

        /* Private properties. */
        private static StringBuilder sb = new();

        /* Public methods. */
        public override string Serialize(TypeNode node, ISerializerScheme scheme)
        {
            OpenCollection(sb, '{');
            AddItem(sb, Tag, node.Name, true, scheme.PrettyPrint, scheme.Tab);
            AddItem(sb, "value", scheme.Serialize(node.Value), false, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb, '}', scheme.PrettyPrint);
            return sb.ToString();
        }

        public override TypeNode Parse(string serialized, ISerializerScheme scheme)
        {
            throw new NotImplementedException();
        }
    }
}