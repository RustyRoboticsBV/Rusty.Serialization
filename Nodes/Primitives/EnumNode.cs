using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Nodes
{
    /// <summary>
    /// A node representing a serialized enum value.
    /// Format: (EnumTypeName:MemberName)
    /// </summary>
    public readonly struct EnumNode : INode
    {
        /* Fields. */
        public readonly string typeName;
        public readonly string memberName;

        /* Constructors. */
        public EnumNode(string typeName, string memberName)
        {
            this.typeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            this.memberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
        }

        /* Public methods. */
        public override string ToString()
        {
            return "enum: " + typeName + "." + memberName;
        }

        public string Serialize() => $"({typeName}:{memberName})";

        /// <summary>
        /// Deserialize from a string of the form (EnumTypeName:MemberName)
        /// </summary>
        public static EnumNode Deserialize(string text)
        {
            string trimmed = text?.Trim() ?? throw new ArgumentException("Cannot parse null.");

            try
            {
                // Enforce parentheses.
                if (!trimmed.StartsWith('(') || !trimmed.EndsWith(')'))
                    throw new ArgumentException("Missing parentheses.");

                // Get text between parentheses and trim it.
                string contents = text.Substring(1, text.Length - 2).Trim();

                // Split into type name and member name.
                List<string> parsed = ParseUtility.Split(contents, ':');

                // Parse terms.
                if (parsed.Count != 2)
                    throw new Exception("Malformed enum.");

                string typeName = parsed[0].Trim();
                if (!IsValidIdentifier(typeName))
                    throw new ArgumentException("Malformed enum type name.");

                string memberName = parsed[1].Trim();
                if (!IsValidIdentifier(memberName))
                    throw new ArgumentException("Malformed enum member name.");

                return new EnumNode(typeName, memberName);
            }
            catch
            {
                throw new ArgumentException($"Could not parse string '{text}' as an enum.");
            }
        }

        /* Private methods. */
        private static bool IsValidIdentifier(string name)
        {
            // No empty or null strings allowed.
            if (string.IsNullOrEmpty(name))
                return false;

            // First character must be a letter or underscore.
            char first = name[0];
            if (!(char.IsLetter(first) || first == '_'))
                return false;

            // Remaining characters can be letters, digits, or underscores.
            for (int i = 1; i < name.Length; i++)
            {
                char c = name[i];
                if (!(char.IsLetterOrDigit(c) || c == '_'))
                    return false;
            }

            return true;
        }
    }
}
