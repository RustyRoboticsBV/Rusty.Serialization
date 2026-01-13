using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON string serializer.
    /// </summary>
    public class ColorSerializer : Serializer<ColorNode>
    {
        /* Public methods. */
        public override string Serialize(ColorNode node, ISerializerScheme scheme)
        {
            return $"\"{node.Value}\"";
        }

        public override ColorNode Parse(string text, ISerializerScheme scheme)
        {
            return (ColorNode)scheme.ParseAsNode(text);
        }
    }
}