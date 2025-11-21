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
    private readonly TypeName typeName;
    private readonly KeyValuePair<Identifier, INode>[] members;

    /* Public properties. */
    public readonly ReadOnlySpan<KeyValuePair<Identifier, INode>> Members => members;

    /* Constructors. */
    public ObjectNode(KeyValuePair<Identifier, INode>[] members)
    {
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
        // Handle empty objects.
        if (members.Length == 0)
            return "<>";

        // Add members.
        string body = string.Join(",",
            members.Select(pair =>
                pair.Key + ":" + pair.Value.Serialize()
            )
        );

        // Surround with pointy brackets.
        return $"<{body}>";
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

            // Handle empty objects.
            if (terms.Count == 0)
                return new ObjectNode(null);

            // Handle key:value pairs.
            var pairs = new KeyValuePair<Identifier, INode>[terms.Count];
            for (int i = 0; i < terms.Count; i++)
            {
                // Split into key and value.
                List<string> pairStrs = ParseUtility.Split(terms[i], ':');
                if (pairStrs.Count != 2)
                    throw new Exception($"Malformed key-value pair.");

                // Get keys and values.
                Identifier key = pairStrs[0].Trim();
                string valueStr = pairStrs[1].Trim();
                INode valueNode = ParseUtility.ParseValue(valueStr);

                // Add key-value pair.
                pairs[i] = new KeyValuePair<Identifier, INode>(key, valueNode);
            }

            return new ObjectNode(pairs);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as an object:\n\n{ex}");
        }
    }
}