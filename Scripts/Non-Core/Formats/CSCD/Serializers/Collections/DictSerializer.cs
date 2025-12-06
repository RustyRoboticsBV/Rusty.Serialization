using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD dictionary serializer.
    /// </summary>
    public class DictSerializer : Serializer<DictNode>
    {
        /* Public methods. */
        public override string Serialize(DictNode node, ISerializerScheme scheme)
        {
            if (node.Pairs == null)
                throw new Exception("Cannot serialize dictionary nodes whose pairs array are null.");

            if (node.Pairs.Length == 0)
                return "{}";

            StringBuilder sb = new();
            for (int i = 0; i < node.Pairs.Length; i++)
            {
                if (i > 0)
                    sb.Append(',');
                sb.Append(scheme.Serialize(node.Pairs[i].Key));
                sb.Append(':');
                sb.Append(scheme.Serialize(node.Pairs[i].Value));
            }
            return '{' + sb.ToString() + '}';
        }

        public override DictNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim() ?? throw new ArgumentException("Cannot parse null.");

            try
            {
                // Enforce curly braces.
                if (!trimmed.StartsWith('{') || !trimmed.EndsWith('}'))
                    throw new Exception("Missing curly braces.");

                // Get text between curly braces and trim it.
                string contents = trimmed.Substring(1, trimmed.Length - 2).Trim();

                // Split into terms.
                List<string> parsed = ParseUtility.Split(contents);

                // Parse terms.
                var pairs = new KeyValuePair<INode, INode>[parsed.Count];
                for (int i = 0; i < parsed.Count; i++)
                {
                    // Split into key and value.
                    List<string> pairStrs = ParseUtility.Split(parsed[i], ':');
                    if (pairStrs.Count != 2)
                        throw new Exception("Malformed key-index pair.");

                    // Parse keys and values.
                    INode key = scheme.Parse(pairStrs[0].Trim());
                    INode value = scheme.Parse(pairStrs[1].Trim());
                    pairs[i] = new(key, value);
                }

                return new DictNode(pairs);

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a dictionary:\n\n{ex.Message}.");
            }
        }
    }
}