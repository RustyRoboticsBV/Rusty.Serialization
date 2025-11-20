using System;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A type label serializer node.
/// </summary>
public readonly struct TypeNode : INode
{
    /* Fields. */
    private readonly TypeName value;

    /* Public properties. */
    public readonly TypeName Value => value;

    /* Constructors. */
    public TypeNode(TypeName value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "type: " + value;
    }

    public readonly string Serialize()
    {
        return $"({value})";
    }

    public static TypeNode Deserialize(string text)
    {
        // Trim trailing whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Enforce square brackets.
            if (!trimmed.StartsWith('(') || !trimmed.EndsWith(')'))
                throw new Exception("Missing parentheses.");

            // Get text between square brackets and trim it.
            string contents = trimmed.Substring(1, trimmed.Length - 2).Trim();

            // Return type node.
            return new(contents);
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as a type.");
        }
    }
}