using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML bool serializer.
    /// </summary>
    public class BoolSerializer : Serializer<BoolNode>
    {
        /* Public methods. */
        public override string Serialize(BoolNode node, ISerializerScheme scheme)
        {
            return XmlUtility.Pack(node.Value ? "true" : "false", "bool");
        }

        public override BoolNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Unpack XML tag.
                string contents = XmlUtility.Unpack(trimmed, "bool");

                // Make sure it's either true or false.
                if (contents.Length == 4 && contents == "true")
                    return new(true);
                if (contents.Length == 5 && contents == "false")
                    return new(false);
                throw new ArgumentException($"Not a valid boolean.");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a boolean:\n{ex.Message}");
            }
        }
    }
}