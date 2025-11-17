using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A real number serializer node.
/// </summary>
public struct Float : INode
{
    /* Fields. */
    private readonly decimal value;

    /* Constructors. */
    public Float(float value) : this((decimal)value) { }
    public Float(double value) : this((decimal)value) { }
    public Float(decimal value)
    {
        this.value = value;
    }

    /* Conversion operators. */
    public static implicit operator Float(float value) => new((decimal)value);
    public static implicit operator Float(double value) => new((decimal)value);
    public static implicit operator Float(decimal value) => new(value);

    public static implicit operator float(Float node) => (float)node.value;
    public static implicit operator double(Float node) => (double)node.value;
    public static implicit operator decimal(Float node) => node.value;

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

    public static Float Deserialize(string text)
    {
        try
        {
            return new(decimal.Parse(text, CultureInfo.InvariantCulture));
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as a float.");
        }
    }
}