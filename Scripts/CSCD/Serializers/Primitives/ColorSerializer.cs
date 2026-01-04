using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD string serializer.
    /// </summary>
    public class ColorSerializer : Serializer<ColorNode>
    {
        /* Public methods. */
        public override string Serialize(ColorNode node, ISerializerScheme scheme)
        {
            return $"#{node.Value}";
        }

        public override ColorNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce leading hex sign.
                if (!trimmed.StartsWith('#'))
                    throw new ArgumentException("Missing hex sign.");

                // Create color string.
                return new ColorNode(trimmed.Substring(1));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a color:\n{ex.Message}");
            }
        }
    }
}