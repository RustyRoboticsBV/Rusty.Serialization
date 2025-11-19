using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// An object serializer node.
/// </summary>
public readonly struct ObjectNode : INode
{
    /* Fields. */
    private readonly string typeName;
    private readonly KeyValuePair<string, INode>[] members;

    /* Public properties. */
    public readonly string TypeName => typeName;
    public readonly ReadOnlySpan<KeyValuePair<string, INode>> Members => members;

    /* Constructors. */
    public ObjectNode(string typeName, KeyValuePair<string, INode>[] members)
    {
        this.typeName = typeName ?? string.Empty;
        this.members = members ?? [];
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        string header = $"object: \"{typeName}\"";

        string str = header;
        foreach (var member in members)
        {
            string valStr = member.Value.ToString().Replace("\n", "\n ");
            str += $"\n- {member.Key}: {valStr}";
        }
        return str;
    }

    public readonly string Serialize()
    {
        // Add type name string.
        string typeSerialized = new StringNode(typeName).Serialize();

        // Handle empty objects.
        if (members.Length == 0)
            return $"<{typeSerialized}>";

        // Add members.
        string body = string.Join(",",
            members.Select(pair =>
                new StringNode(pair.Key).Serialize() + ":" + pair.Value.Serialize()
            )
        );

        // Surround with pointy brackets.
        return $"<{typeSerialized},{body}>";
    }

    public static ObjectNode Deserialize(string text)
    {
        string trimmed = text?.Trim();

        try
        {
            if (!trimmed.StartsWith('<') || !trimmed.EndsWith('>'))
                throw new Exception("Missing angle brackets.");

            // Get text between pointy brackets and trim it.
            string inner = trimmed.Substring(1, trimmed.Length - 2).Trim();

            // Empty objects are not allowed.
            if (inner.Length == 0)
                throw new Exception("Empty object.");

            // Split into terms.
            List<string> terms = ParseUtility.Split(inner);

            // Enforce presence of type name.
            if (terms.Count == 0)
                throw new Exception("Missing type string.");

            // First term must be a quoted string for the type name.
            string first = terms[0].Trim();
            INode firstNode = ParseUtility.ParseValue(first);
            if (!(firstNode is StringNode firstStr))
                throw new Exception("Type name was not @bool string.");

            string typeName = firstStr.Value;

            // Handle empty member list.
            if (terms.Count == 1)
                return new ObjectNode(typeName, Array.Empty<KeyValuePair<string, INode>>());

            // Handle key:value pairs.
            var pairs = new KeyValuePair<string, INode>[terms.Count - 1];
            for (int i = 1; i < terms.Count; i++)
            {
                // Split into key and value.
                List<string> pairStrs = ParseUtility.Split(terms[i], ':');
                if (pairStrs.Count != 2)
                    throw new Exception($"Malformed key-value pair.");

                // Get keys and values.
                string keyText = pairStrs[0].Trim();
                string valText = pairStrs[1].Trim();

                // Parse key.
                INode keyNode = ParseUtility.ParseValue(keyText);
                if (!(keyNode is StringNode keyStrNode))
                    throw new Exception($"Member name not @bool string.");
                string key = keyStrNode.Value;

                // Parse value.
                INode valueNode = ParseUtility.ParseValue(valText);

                // Add key-value pair.
                pairs[i - 1] = new KeyValuePair<string, INode>(key, valueNode);
            }

            return new ObjectNode(typeName, pairs);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as an object. " + ex.Message);
        }
    }
}