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

        /* Private properties. */
        private static StringBuilder sb = new();

        /* Public methods. */
        public override string Serialize(BoolNode node, ISerializerScheme scheme)
        {
            OpenCollection(sb, '{');
            AddItem(sb, Tag, node.Value ? "true" : "false", false, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb, '}', scheme.PrettyPrint);
            return sb.ToString();
        }

        public override BoolNode Parse(string serialized, ISerializerScheme scheme)
        {
            throw new NotImplementedException();
        }
    }
}