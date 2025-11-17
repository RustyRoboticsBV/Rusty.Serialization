using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// An object serializer node.
/// </summary>
public struct Object : INode
{
    /* Fields. */
    private readonly string typeName;
    private readonly KeyValuePair<string, INode>[] members;

    /* Constructors. */
    public Object(string typeName, KeyValuePair<string, INode>[] members)
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
        string typeSerialized = ((String)typeName).Serialize();

        // Handle empty objects.
        if (members.Length == 0)
            return $"<{typeSerialized}>";

        // Add members.
        string body = string.Join(",",
            members.Select(kv =>
                ((String)kv.Key).Serialize() + ":" + kv.Value.Serialize()
            )
        );

        // Surround with pointy brackets.
        return $"<{typeSerialized},{body}>";
    }

    public static Object Deserialize(string text)
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
            if (!(firstNode is String firstStr))
                throw new Exception("Type name was not a string.");

            string typeName = (string)firstStr;

            // Handle empty member list.
            if (terms.Count == 1)
                return new Object(typeName, Array.Empty<KeyValuePair<string, INode>>());

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
                if (!(keyNode is String keyStrNode))
                    throw new Exception($"Member name not a string.");
                string key = (string)keyStrNode;

                // Parse value.
                INode valueNode = ParseUtility.ParseValue(valText);

                // Add key-value pair.
                pairs[i - 1] = new KeyValuePair<string, INode>(key, valueNode);
            }

            return new Object(typeName, pairs);
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as an object.");
        }
    }
}