using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML int serializer.
    /// </summary>
    public class IntSerializer : Serializer<IntNode>
    {
        /* Public methods. */
        public override string Serialize(IntNode node, ISerializerScheme scheme)
        {
            return XmlUtility.Pack(node.Value.ToString(CultureInfo.InvariantCulture), "int");
        }

        public override IntNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Unpack XML.
                string contents = XmlUtility.Unpack(trimmed, "int");

                // Parse.
                return new(decimal.Parse(contents, CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as an integer:\n{ex.Message}");
            }
        }
    }
}