using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A real number serializer node.
/// </summary>
public readonly struct FloatNode : INode
{
    /* Fields. */
    private readonly decimal value;

    /* Public properties. */
    public readonly decimal Value => value;

    /* Constructors. */
    public FloatNode(decimal value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "float: " + value;
    }

    public readonly string Serialize()
    {
        string text = value.ToString(CultureInfo.InvariantCulture);
        if (!text.Contains('.'))
            text += ".";
        while (text.EndsWith('0'))
        {
            text = text.Substring(0, text.Length - 1);
        }
        if (text.EndsWith('.'))
            text += '0';
        return text;
    }

    public static FloatNode Deserialize(string text)
    {
        try
        {
            return new(decimal.Parse(text, CultureInfo.InvariantCulture));
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as @bool float.");
        }
    }
}