using System;
using System.Collections.Generic;
using System.Text;
    
namespace Rusty.Serialization.Nodes;

/// <summary>
/// An object serializer node.
/// </summary>
public readonly struct ObjectNode : INode
{
    /* Fields. */
    private readonly KeyValuePair<string, INode>[] members;

    /* Public properties. */
    public readonly ReadOnlySpan<KeyValuePair<string, INode>> Members => members;

    /* Constructors. */
    public ObjectNode(KeyValuePair<string, INode>[] members)
    {
        this.members = members ?? [];
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        string header = $"object:";

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
        StringBuilder body = new();
        for (int i = 0; i < members.Length; i++)
        {
            if (i > 0)
                body.Append(',');
            ValidateIdentifier(members[i].Key);
            body.Append(members[i].Key);
            body.Append(':');
            body.Append(members[i].Value.Serialize());
        }

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

            // Split into terms.
            List<string> terms = ParseUtility.Split(inner);

            // Handle empty objects.
            if (terms.Count == 0)
                return new ObjectNode(null);

            // Handle key:value pairs.
            var pairs = new KeyValuePair<string, INode>[terms.Count];
            for (int i = 0; i < terms.Count; i++)
            {
                // Split into key and value.
                List<string> pairStrs = ParseUtility.Split(terms[i], ':');
                if (pairStrs.Count != 2)
                    throw new Exception($"Malformed identifier-name pair.");

                // Get identifier.
                string identifier = pairStrs[0].Trim();
                ValidateIdentifier(identifier);

                // Get value.
                string valueStr = pairStrs[1].Trim();
                INode valueNode = ParseUtility.ParseValue(valueStr);

                // Add key-value pair.
                pairs[i] = new KeyValuePair<string, INode>(identifier, valueNode);
            }

            return new ObjectNode(pairs);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as an object:\n\n{ex.Message}");
        }
    }

    /* Private methods. */
    private static void ValidateIdentifier(string identifier)
    {
        for (int i = 0; i < identifier.Length; i++)
        {
            char c = identifier[i];
            if (i == 0)
            {
                if (!(c <= '~' && (c == '_' || char.IsLetter(c))))
                    throw new ArgumentException($"Illegal identifier '{identifier}' (must start with letter or underscore).");
            }
            else if (!(c <= '~' && (c == '_' || char.IsLetter(c) || char.IsDigit(c))))
                throw new ArgumentException($"Illegal identifier '{identifier}' (must only contain letters, digits or underscores).");
        }
    }
}