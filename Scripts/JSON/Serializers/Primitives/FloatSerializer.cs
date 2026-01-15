using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON float serializer.
    /// </summary>
    public class FloatSerializer : Serializer<FloatNode>
    {
        /* Public methods. */
        public override string Serialize(FloatNode node, ISerializerScheme scheme)
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

        public override FloatNode Parse(string text, ISerializerScheme scheme)
        {
            return new FloatNode(text?.Trim());
        }
    }
}