using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD bool serializer.
    /// </summary>
    public class BoolSerializer : Serializer<BoolNode>
    {
        /* Public methods. */
        public override string Serialize(BoolNode node, ISerializerScheme scheme)
        {
            return node.Value ? "true" : "false";
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

                // Make sure it's either true or false.
                if (trimmed.Length == 4 && trimmed == "true")
                    return new(true);
                if (trimmed.Length == 5 && trimmed == "false")
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