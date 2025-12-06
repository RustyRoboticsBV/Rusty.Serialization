using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD ID serializer.
    /// </summary>
    public class IdSerializer : Serializer<IdNode>
    {
        /* Public methods. */
        public override string Serialize(IdNode node, ISerializerScheme scheme)
        {
            Validate(node.Name);
            return '`' + node.Name.ToString(CultureInfo.InvariantCulture) + '`'
                + (scheme.PrettyPrint ? " " : "") + scheme.Serialize(node.Value);
        }

        public override IdNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce parentheses.
                int closeIndex = -1;
                for (int i = 1; i < trimmed.Length; i++)
                {
                    if (trimmed[i] == '`')
                    {
                        closeIndex = i;
                        break;
                    }
                }

                if (!trimmed.StartsWith('`') || closeIndex == -1)
                    throw new ArgumentException("Missing backticks.");

                // Get text between backticks, validate and trim it.
                string name = trimmed.Substring(1, closeIndex - 1).Trim();
                Validate(name);

                // Get text after backticks and parse it.
                string value = trimmed.Substring(closeIndex + 1).Trim();
                INode valueNode = null;
                if (value.Length > 0)
                    valueNode = scheme.ParseAsNode(value);

                // Parse.
                return new(name, valueNode);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as an ID:\n{ex.Message}");
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