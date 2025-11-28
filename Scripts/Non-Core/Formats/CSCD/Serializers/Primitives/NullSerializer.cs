using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD null serializer.
    /// </summary>
    public class NullSerializer : Serializer<NullNode>
    {
        /* Public methods. */
        public override string Serialize(NullNode node, ISerializerScheme scheme)
        {
            return "null";
        }

        public override NullNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Make sure the text is equal to null.
                if (trimmed != "null")
                    throw new ArgumentException("Bad literal.");

                return new();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a null name:\n{ex.Message}");
            }
        }
    }
}