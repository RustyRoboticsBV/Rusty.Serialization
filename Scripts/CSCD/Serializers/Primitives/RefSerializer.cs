using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD reference serializer.
    /// </summary>
    public class RefSerializer : Serializer<RefNode>
    {
        /* Public methods. */
        public override string Serialize(RefNode node, ISerializerScheme scheme)
        {
            Validate(node.ID);
            return '&' + node.ID.ToString(CultureInfo.InvariantCulture);
        }

        public override RefNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce ampersand.
                if (!trimmed.StartsWith('&'))
                    throw new ArgumentException("Missing ampersand.");
                string contents = trimmed.Substring(1).Trim();

                // Validate characters.
                Validate(contents);

                // Parse.
                return new(contents);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a reference:\n{ex.Message}");
            }
        }

        private static void Validate(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!IsLegalChar(str[i]))
                    throw new ArgumentException($"Illegal character '{str[i]}' at {i} in {str}.");
            }
        }

        private static bool IsLegalChar(char c)
        {
            return (c >= 0 && c <= '9')
                || (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || c == '_';
        }
    }
}