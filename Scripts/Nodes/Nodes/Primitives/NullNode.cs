using System;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A null serializer node.
/// </summary>
public readonly struct NullNode : INode
{
    /* Public methods. */
    public override readonly string ToString()
    {
        return "null";
    }

    public readonly string Serialize()
    {
        return "null";
    }

    public static NullNode Parse(string text)
    {
        // Remove whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Make sure the text is equal to null.
            if (trimmed != "null")
                throw new ArgumentException("Bad literal.");

            return new();
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a null value:\n{ex.Message}");
        }
    }
}