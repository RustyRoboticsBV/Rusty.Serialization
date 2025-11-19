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

    public static BoolNode Deserialize(string text)
    {
        try
        {
            return new(bool.Parse(text));
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as a boolean.");
        }
    }
}