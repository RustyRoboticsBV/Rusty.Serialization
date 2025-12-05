using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class CharSerializer : JsonSerializer<CharNode>
    {
        /* Public properties. */
        public override string Tag => "char";

        /* Private properties. */
        private static StringBuilder sb = new();

        /* Public methods. */
        public override string Serialize(CharNode node, ISerializerScheme scheme)
        {
            OpenCollection(sb, '{');
            AddItem(sb, Tag, node.Value <= char.MaxValue ? ((char)node.Value).ToString() : throw new Exception(), true, scheme.PrettyPrint, scheme.Tab);
            CloseCollection(sb, '}', scheme.PrettyPrint);
            return sb.ToString();
        }

        public override CharNode Parse(string serialized, ISerializerScheme scheme)
        {
            throw new NotImplementedException();
        }
    }
}