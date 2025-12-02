using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML binary serializer.
    /// </summary>
    public class BinarySerializer : Serializer<BinaryNode>
    {
        /* Public methods. */
        public override string Serialize(BinaryNode node, ISerializerScheme scheme)
        {
            return XmlUtility.Pack(HexUtility.ToHexString(node.Value), "bin");
        }

        public override BinaryNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Unpack XML.
                string contents = XmlUtility.Unpack(trimmed, "bin");

                // Enforce even length.
                if (contents.Length % 2 != 0)
                    throw new ArgumentException("Binary literals must have an even length.");

                // Parse as byte array.
                byte[] bytes = HexUtility.BytesFromHexString(contents);

                return new(bytes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a binary:\n{ex.Message}");
            }
        }
    }
}