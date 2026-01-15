using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON decimal serializer.
    /// </summary>
    public class DecimalSerializer : Serializer<DecimalNode>
    {
        /* Public methods. */
        public override string Serialize(DecimalNode node, ISerializerScheme scheme)
        {
            // Parse raw decimal.
            string text = node.Value.ToString();

            // Avoid leading/trailing decimal point.
            if (text.StartsWith('.'))
                text = '0' + text;
            if (text.EndsWith('.'))
                text = text.Substring(0, text.Length - 1);

            return text;
        }

        public override DecimalNode Parse(string text, ISerializerScheme scheme)
        {
            return (DecimalNode)scheme.ParseAsNode(text);
        }
    }
}