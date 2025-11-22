using System;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A boolean serializer node.
/// </summary>
public readonly struct BoolNode : INode
{
    /* Fields. */
    private readonly bool value;

    /* Public properties. */
    public readonly bool Value => value;

    /* Constructors. */
    public BoolNode(bool value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "bool: " + value;
    }

    public readonly string Serialize()
    {
        return value ? "true" : "false";
    }

    public static BoolNode Parse(string text)
    {
        // Remove whitespaces.
        string trimmed = text?.Trim();

        try
        {
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");
            if (trimmed.Length == 4 && trimmed == "true")
                return new(true);
            if (trimmed.Length == 5 && trimmed == "false")
                return new(false);
            throw new ArgumentException($"Not a valid boolean.");
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a boolean:\n{ex.Message}");
        }
    }
}