using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON real serializer.
    /// </summary>
    public class RealSerializer : Serializer<RealNode>
    {
        /* Public methods. */
        public override string Serialize(RealNode node, ISerializerScheme scheme)
        {
            // Parse raw decimal.
            string text = node.Value.ToString();

            // Avoid leading/trailing decimal point.
            if (text.StartsWith('.'))
                text = '0' + text;
            if (text.EndsWith('.'))
                text = text + '0';

            return text;
        }

        public override RealNode Parse(string text, ISerializerScheme scheme)
        {
            return new RealNode(text?.Trim());
        }
    }
}