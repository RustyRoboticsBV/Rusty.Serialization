using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON int serializer.
    /// </summary>
    public class IntSerializer : Serializer<IntNode>
    {
        /* Public methods. */
        public override string Serialize(IntNode node, ISerializerScheme scheme)
        {
            return node.Value;
        }

        public override IntNode Parse(string text, ISerializerScheme scheme)
        {
            return new IntNode(text?.Trim());
        }
    }
}