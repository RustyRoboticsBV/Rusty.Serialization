using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON object serializer.
    /// </summary>
    public class ObjectSerializer : Serializer<ObjectNode>
    {
        /* Public methods. */
        public override string Serialize(ObjectNode node, ISerializerScheme scheme)
        {
            if (node.Members == null)
                throw new Exception("Cannot serialize object nodes whose members array are null.");

            if (node.Members.Length == 0)
                return "<>";

            bool prettyPrint = scheme.PrettyPrint;
            string tab = scheme.Tab;

            // Add members.
            StringBuilder sb = new();
            for (int i = 0; i < node.Members.Length; i++)
            {
                string value = scheme.Serialize(node.Members[i].Value);

                if (prettyPrint)
                    sb.Append('\n' + tab);

                // Key.
                ValidateIdentifier(node.Members[i].Key);
                sb.Append(node.Members[i].Key);

                // Separator.
                if (prettyPrint)
                    sb.Append(' ');
                sb.Append(':');
                if (prettyPrint)
                    sb.Append(' ');

                // Value.
                if (prettyPrint && i < node.Members.Length - 1)
                    sb.Append(value.Replace("\n", "\n" + tab));
                else
                    sb.Append(value);

                // Comma.
                if (i < node.Members.Length - 1)
                    sb.Append(',');
            }
            if (prettyPrint)
                sb.Append('\n');
            return $"<{sb}>";
        }

        public override ObjectNode Parse(string text, ISerializerScheme scheme)
        {
            string trimmed = text?.Trim();

            try
            {
                if (!trimmed.StartsWith('<') || !trimmed.EndsWith('>'))
                    throw new Exception("Missing angle brackets.");

                // Get text between pointy brackets and trim it.
                string inner = trimmed.Substring(1, trimmed.Length - 2).Trim();

                // Split into terms.
                List<string> terms = ParseUtility.Split(inner);

                // Handle empty objects.
                if (terms.Count == 0)
                    return new ObjectNode(null);

                // Handle key:value pairs.
                var pairs = new KeyValuePair<string, INode>[terms.Count];
                for (int i = 0; i < terms.Count; i++)
                {
                    // Split into key and value.
                    List<string> pairStrs = ParseUtility.Split(terms[i], ':');
                    if (pairStrs.Count != 2)
                        throw new Exception($"Malformed identifier-index pair.");

                    // Get identifier.
                    string identifier = pairStrs[0].Trim();
                    ValidateIdentifier(identifier);

                    // Get value.
                    string valueStr = pairStrs[1].Trim();
                    INode valueNode = scheme.ParseAsNode(valueStr);

                    // Add key-value pair.
                    pairs[i] = new KeyValuePair<string, INode>(identifier, valueNode);
                }

                return new ObjectNode(pairs);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as an object:\n\n{ex.Message}");
            }
        }

        /* Private methods. */
        private static void ValidateIdentifier(string identifier)
        {
            for (int i = 0; i < identifier.Length; i++)
            {
                char c = identifier[i];
                if (i == 0)
                {
                    if (!(c <= '~' && (c == '_' || char.IsLetter(c))))
                        throw new ArgumentException($"Illegal identifier '{identifier}' (must start with letter or underscore).");
                }
                else if (!(c <= '~' && (c == '_' || char.IsLetter(c) || char.IsDigit(c))))
                    throw new ArgumentException($"Illegal identifier '{identifier}' (must only contain letters, digits or underscores).");
            }
        }
    }
}