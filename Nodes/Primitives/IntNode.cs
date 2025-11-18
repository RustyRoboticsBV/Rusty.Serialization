using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// An integer serializer node.
/// </summary>
public readonly struct IntNode : INode
{
    /* Fields. */
    private readonly decimal value;

    /* Public properties. */
    public readonly decimal Value => value;

    /* Constructors. */
    public IntNode(decimal value)
    {
        if (value < 0)
            this.value = (long)value;
        else
            this.value = (ulong)value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "int: " + value;
    }

    public readonly string Serialize()
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static IntNode Deserialize(string text)
    {
        try
        {
            return new(decimal.Parse(text, CultureInfo.InvariantCulture));
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as an integer.");
        }
    }
}