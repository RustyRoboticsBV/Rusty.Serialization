using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON dictionary serializer.
    /// </summary>
    public class DictSerializer : Serializer<DictNode>
    {
        /* Public methods. */
        public override string Serialize(DictNode node, ISerializerScheme scheme)
        {
            return ""; // TODO: implement.
        }

        public override DictNode Parse(string text, ISerializerScheme scheme)
        {
            return (DictNode)scheme.ParseAsNode(text);
        }
    }
}