using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD infinity serializer.
    /// </summary>
    public class InfinitySerializer : Serializer<InfinityNode>
    {
        /* Public methods. */
        public override string Serialize(InfinityNode node, ISerializerScheme scheme)
        {
            return node.Positive ? "inf" : "-inf";
        }

        public override InfinityNode Parse(string text, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));
            
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Make sure it's either positive or negative infinity.
                if (trimmed.Length == 3 && trimmed == "inf")
                    return new(true);
                if (trimmed.Length == 4 && trimmed == "-inf")
                    return new(false);
                throw new ArgumentException($"Not a valid infinity.");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as an infinity:\n{ex.Message}");
            }
        }
    }
}