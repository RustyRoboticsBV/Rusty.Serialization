using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML real serializer.
    /// </summary>
    public class RealSerializer : Serializer<RealNode>
    {
        /* Public methods. */
        public override string Serialize(RealNode node, ISerializerScheme scheme)
        {
            // Parse raw decimal.
            string text = node.Value.ToString();

            // To xml.
            return XmlUtility.Pack(text, "real");
        }

        public override RealNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce <real> and </real>.
                string contents = XmlUtility.Unpack(trimmed, "real");

                // Parse.
                return new(PeterO.Numbers.EDecimal.FromString(trimmed));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a float:\n{ex.Message}");
            }
        }
    }
}