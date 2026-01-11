using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON bytes serializer.
    /// </summary>
    public class BytesSerializer : Serializer<BytesNode>
    {
        /* Private constants. */
        private const string Prefix = "b_";

        /* Public methods. */
        public override string Serialize(BytesNode node, ISerializerScheme scheme)
        {
            return $"{Prefix}{node.Value}";
        }

        public override BytesNode Parse(string text, ISerializerScheme scheme)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Enforce prefix.
                if (!trimmed.StartsWith(Prefix))
                    throw new ArgumentException($"Missing '{Prefix}' prefix.");

                // Get contents.
                string contents = trimmed.Substring(Prefix.Length);

                // Enforce length multiple of 4.
                if (contents.Length % 4 != 0)
                    throw new ArgumentException("Bytes literals must have a length that is a multiple of 4.");

                return new(contents);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a binary:\n{ex.Message}");
            }
        }
    }
}