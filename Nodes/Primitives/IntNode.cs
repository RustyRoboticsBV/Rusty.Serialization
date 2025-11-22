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

    public static IntNode Parse(string text)
    {
        string trimmed = text?.Trim();
        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Check syntax.
            for (int i = 0; i < trimmed.Length; i++)
            {
                if (!(i == 0 && trimmed[i] == '-' || trimmed[i] >= '0' && trimmed[i] <= '9'))
                    throw new ArgumentException($"Illegal character '{trimmed[i]}' at {i}.");
            }

            // Parse.
            return new(decimal.Parse(trimmed, CultureInfo.InvariantCulture));
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as an integer:\n{ex.Message}");
        }
    }
}