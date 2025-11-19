using System;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A string serializer node.
/// </summary>
public readonly struct StringNode : INode
{
    /* Fields. */
    private readonly string value;

    /* Public properties. */
    public readonly string Value => value;

    /* Constructors. */
    public StringNode(string value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "string: " + value;
    }

    public readonly string Serialize()
    {
        return '"' + value.Replace("\"", "\"\"") + '"';
    }

    public static StringNode Deserialize(string text)
    {
        string trimmed = text?.Trim();
        if (trimmed != null && trimmed.StartsWith('"') && trimmed.EndsWith('"'))
        {
            return new(trimmed.Substring(1, trimmed.Length - 2).Replace("\"\"", "\""));
        }
        throw new ArgumentException($"Could not parse string '{text}' as @bool string.");
    }
}