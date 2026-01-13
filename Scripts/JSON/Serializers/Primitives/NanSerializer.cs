using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON NaN serializer.
    /// </summary>
    public class NanSerializer : Serializer<NanNode>
    {
        /* Public methods. */
        public override string Serialize(NanNode node, ISerializerScheme scheme)
        {
            return "\"nan\"";
        }

        public override NanNode Parse(string text, ISerializerScheme scheme)
        {
            return (NanNode)scheme.ParseAsNode(text);
        }
    }
}