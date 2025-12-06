using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD type serializer.
    /// </summary>
    public class TypeSerializer : Serializer<TypeNode>
    {
        /* Public methods. */
        public override string Serialize(TypeNode node, ISerializerScheme scheme)
        {
            string name = node.Name.Trim();
            Validate(name);

            if (node.Value == null)
                throw new InvalidOperationException("index was null.");
            return $"({name}){scheme.Serialize(node.Value)}";
        }

        public override TypeNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce parentheses.
                int closeIndex = trimmed.IndexOf(')');
                if (!trimmed.StartsWith('(') || closeIndex == -1)
                    throw new ArgumentException("Missing parentheses.");

                // Get text between parentheses, validate and trim it.
                string name = trimmed.Substring(1, closeIndex - 1).Trim();
                Validate(name);

                // Get text after parentheses and parse it.
                string value = trimmed.Substring(closeIndex + 1).Trim();
                INode valueNode = null;
                if (value.Length > 0)
                    valueNode = scheme.ParseAsNode(value);

                // Return type node.
                return new(name, valueNode);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a type:\n{ex.Message}");
            }
        }

        /* Private methods. */
        private static void Validate(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (c < '!' && c > '~' || c == '(' || c == ')')
                    throw new ArgumentException($"Illegal character '{c}' in type '{name}'.");
            }
        }
    }
}