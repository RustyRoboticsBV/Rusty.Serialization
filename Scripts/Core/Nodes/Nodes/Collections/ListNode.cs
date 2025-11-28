using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A list serializer node.
    /// </summary>
    public readonly struct ListNode : INode
    {
        /* Fields. */
        private readonly INode[] elements;

        /* Public properties. */
        public readonly ReadOnlySpan<INode> Elements => elements;

        /* Constructors. */
        public ListNode(INode[] elements)
        {
            this.elements = elements;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            string str = "list: ";
            for (int i = 0; i < elements.Length; i++)
            {
                str += "\n- " + elements[i].ToString().Replace("\n", "\n  ");
            }
            return str;
        }

        public readonly string Serialize()
        {
            if (elements == null)
                throw new Exception("Cannot serialize list nodes whose elements array are null.");

            if (elements.Length == 0)
                return "[]";

            StringBuilder sb = new();
            for (int i = 0; i < elements.Length; i++)
            {
                if (i > 0)
                    sb.Append(',');
                sb.Append(elements[i].Serialize());
            }
            return '[' + sb.ToString() + ']';
        }

        public static ListNode Deserialize(string text)
        {
            // Trim trailing whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Enforce square brackets.
                if (!trimmed.StartsWith('[') || !trimmed.EndsWith(']'))
                    throw new Exception("Missing square brackets.");

                // Get text between square brackets and trim it.
                string contents = trimmed.Substring(1, trimmed.Length - 2);

                // Split into terms.
                List<string> terms = ParseUtility.Split(contents);

                // Create child nodes.
                INode[] nodes = new INode[terms.Count];
                for (int i = 0; i < terms.Count; i++)
                {
                    nodes[i] = ParseUtility.ParseValue(terms[i]);
                }

                // Create list node.
                return new(nodes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a elements:\n\n{ex.Message}.");
            }
        }
    }
}