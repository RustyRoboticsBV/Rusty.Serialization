using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON infinity serializer.
    /// </summary>
    public class InfinitySerializer : Serializer<InfinityNode>
    {
        /* Public methods. */
        public override string Serialize(InfinityNode node, ISerializerScheme scheme)
        {
            return node.Positive ? "\"inf\"" : "\"-inf\"";
        }

        public override InfinityNode Parse(string text, ISerializerScheme scheme)
        {
            return (InfinityNode)scheme.ParseAsNode(text);
        }
    }
}