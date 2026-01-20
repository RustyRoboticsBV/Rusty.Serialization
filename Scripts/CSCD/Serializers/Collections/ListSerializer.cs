using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD list serializer.
    /// </summary>
    public class ListSerializer : Serializer<ListNode>
    {
        /* Public methods. */
        public override string Serialize(ListNode node, ISerializerScheme scheme)
        {
            if (node.Elements == null)
                throw new Exception("Cannot serialize list nodes whose Elements array are null.");

            if (node.Count == 0)
                return "[]";

            StringBuilder sb = new();
            for (int i = 0; i < node.Count; i++)
            {
                if (i > 0)
                    sb.Append(',');
                sb.Append(scheme.Serialize(node.Elements[i], scheme.PrettyPrint));
            }
            return '[' + sb.ToString() + ']';
        }

        public override ListNode Parse(string text, ISerializerScheme scheme)
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
                    nodes[i] = scheme.ParseAsNode(terms[i]);
                }

                // Create list node.
                return new(nodes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a Elements:\n\n{ex.Message}.");
            }
        }
    }
}