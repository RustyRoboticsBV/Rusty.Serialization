using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A character serializer node.
/// </summary>
public readonly struct CharNode : INode
{
    /* Fields. */
    private readonly char value;

    /* Public properties. */
    public readonly char Value => value;

    /* Constructors. */
    public CharNode(char value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "char: " + value;
    }

    public readonly string Serialize()
    {
        if (value == '\'')
            return "''''";
        else
            return $"'{value.ToString(CultureInfo.InvariantCulture)}'";
    }

    public static CharNode Deserialize(string text)
    {
        string trimmed = text?.Trim();
        if (trimmed != null)
        {
            if (trimmed == "''")
                return new('\0');
            else if (trimmed == "''''")
                return new('\'');
            else if (trimmed.Length == 3 && trimmed.StartsWith('\'') && trimmed.EndsWith('\''))
                return new(trimmed[1]);
        }
        throw new ArgumentException($"Could not parse string '{text}' as a character.");
    }
}