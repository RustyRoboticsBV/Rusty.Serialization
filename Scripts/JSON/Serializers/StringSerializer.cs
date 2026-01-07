using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON string serializer.
    /// </summary>
    public class StringSerializer : Serializer<StringNode>
    {
        /* Public methods. */
        public override string Serialize(StringNode node, ISerializerScheme scheme)
        {
            StringBuilder str = new();
            for (int i = 0; i < node.Value.Length; i++)
            {
                char c = node.Value[i];

                // Escaped characters.
                if (c == '\\')
                    str.Append("\\\\");
                else if (c == '"')
                    str.Append("\\\"");
                else if (c == '\t')
                    str.Append("\\t");
                else if (c == '\n')
                    str.Append("\\n");

                // Handle unicode characters.
                else if (!CSCD.CharUtility.Check(c))
                    str.Append("\\u" + CSCD.UnicodeUtility.CodePointToHex(c));

                // Otherwise, append character as-is.
                else
                    str.Append(node.Value[i]);
            }

            // Enclose in double quotes.
            return '"' + str.ToString() + '"';
        }

        public override StringNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce double quotes.
                if (!trimmed.StartsWith('"') || !trimmed.EndsWith('"'))
                    throw new ArgumentException("Missing double-quotes.");

                // Extract contents.
                string contents = trimmed.Substring(1, trimmed.Length - 2);

                // Convert from the JSON string.
                string packed = CSCD.StringUtility.Parse(contents, '"');

                // Return finished node.
                return new StringNode(packed);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a string:\n{ex.Message}");
            }
        }
    }
}