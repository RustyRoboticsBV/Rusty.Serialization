using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON bytes serializer.
    /// </summary>
    public class BytesSerializer : Serializer<BytesNode>
    {
        /* Public methods. */
        public override string Serialize(BytesNode node, ISerializerScheme scheme)
        {
            return $"\"{node.Value}\"";
        }

        public override BytesNode Parse(string text, ISerializerScheme scheme)
        {
            return (BytesNode)scheme.ParseAsNode(text);
        }
    }
}