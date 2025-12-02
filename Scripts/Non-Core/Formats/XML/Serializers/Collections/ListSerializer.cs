using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML list serializer.
    /// </summary>
    public class ListSerializer : Serializer<ListNode>
    {
        /* Public methods. */
        public override string Serialize(ListNode node, ISerializerScheme scheme)
        {
            if (node.Elements == null)
                throw new Exception("Cannot serialize list nodes whose elements array are null.");

            if (node.Elements.Length == 0)
                return XmlUtility.Pack("", "list");

            StringBuilder sb = new();
            for (int i = 0; i < node.Elements.Length; i++)
            {
                if (i > 0)
                    sb.Append('\n');
                sb.Append(scheme.Serialize(node.Elements[i]));
            }
            return XmlUtility.Pack(sb.ToString(), "list");
        }

        public override ListNode Parse(string text, ISerializerScheme scheme)
        {
            // Trim trailing whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Unpack XML.
                string contents = XmlUtility.Unpack(trimmed, "list");

                // Split into terms.
                List<string> terms = ParseUtility.Split(contents);

                // Create child nodes.
                INode[] nodes = new INode[terms.Count];
                for (int i = 0; i < terms.Count; i++)
                {
                    nodes[i] = scheme.Parse(terms[i]);
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