using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON NaN serializer.
    /// </summary>
    public class NanSerializer : Serializer<NanNode>
    {
        /* Public methods. */
        public override string Serialize(NanNode node, ISerializerScheme scheme)
        {
            return "nan";
        }

        public override NanNode Parse(string text, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                if (trimmed != "nan")
                    throw new ArgumentException("Bad literal.");

                return new();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a nan:\n{ex.Message}");
            }
        }
    }
}