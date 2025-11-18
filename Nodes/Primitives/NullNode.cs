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

    public static NullNode Deserialize(string text)
    {
        if (text.Trim().ToLower() == "null")
            return new();
        else
            throw new ArgumentException($"Could not parse string '{text}' as a null value.");
    }
}