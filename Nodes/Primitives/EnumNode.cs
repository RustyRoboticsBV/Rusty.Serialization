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
        public readonly TypeName typeName;
        public readonly Identifier memberName;

        /* Constructors. */
        public EnumNode(TypeName typeName, Identifier memberName)
        {
            this.typeName = typeName;
            this.memberName = memberName;
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

                TypeName typeName = parsed[0].Trim();
                Identifier memberName = parsed[1].Trim();
                return new EnumNode(typeName, memberName);
            }
            catch
            {
                throw new ArgumentException($"Could not parse string '{text}' as an enum.");
            }
        }
    }
}
