using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD int serializer.
    /// </summary>
    public class IntSerializer : Serializer<IntNode>
    {
        /* Public methods. */
        public override string Serialize(IntNode node, ISerializerScheme scheme)
        {
            return node.Value;
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

                // Check syntax.
                for (int i = 0; i < trimmed.Length; i++)
                {
                    if (!((i == 0 && trimmed[i] == '-') || (trimmed[i] >= '0' && trimmed[i] <= '9')))
                        throw new ArgumentException($"Illegal character '{trimmed[i]}' at {i}.");
                }

                // Parse.
                return new(trimmed);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as an integer:\n{ex.Message}");
            }
        }
    }
}