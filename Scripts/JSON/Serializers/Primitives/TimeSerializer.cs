using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON time serializer.
    /// </summary>
    public class TimeSerializer : Serializer<TimeNode>
    {
        /* Public methods. */
        public override string Serialize(TimeNode node, ISerializerScheme scheme)
        {
            return scheme.Serialize((ObjectNode)node, scheme.PrettyPrint);
        }

        public override TimeNode Parse(string text, ISerializerScheme scheme)
        {
            return (TimeNode)scheme.ParseAsNode(text);
        }
    }
}