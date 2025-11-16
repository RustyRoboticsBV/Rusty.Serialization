using System;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A boolean serializer node.
/// </summary>
public struct Boolean : INode
{
    /* Fields. */
    private readonly bool value;

    /* Constructors. */
    private Boolean(bool value)
    {
        this.value = value;
    }

    /* Conversion operators. */
    public static implicit operator Boolean(bool value) => new(value);

    public static implicit operator bool(Boolean node) => node.value;

    /* Public methods. */
    public readonly string Serialize()
    {
        return value ? "true" : "false";
    }

    public static Boolean Deserialize(string text)
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