using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A color serializer node.
/// </summary>
public struct ColorNode : INode
{
    /* Fields. */
    private readonly byte r;
    private readonly byte g;
    private readonly byte b;
    private readonly byte a;

    /* Public properties. */
    public readonly byte R => r;
    public readonly byte G => g;
    public readonly byte B => b;
    public readonly byte A => a;

    /* Constructors. */
    public ColorNode(byte r, byte g, byte b, byte a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return $"gdcolor: ({r},{g},{b},{a})";
    }

    public readonly string Serialize()
    {
        if (a == 255)
            return $"#{r:X2}{g:X2}{b:X2}";
        else
            return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    public static ColorNode Deserialize(string text)
    {
        try
        {
            // Remove whitespaces.
            text = text.Trim();

            // Enforce leading hex sign.
            if (!text.StartsWith('#'))
                throw new Exception();

            // Split into substrs.
            if (text.Length != 7 && text.Length != 9)
                throw new Exception();
            string r = text.Substring(1, 2);
            string g = text.Substring(3, 2);
            string b = text.Substring(5, 2);
            string a = text.Length == 9 ? text.Substring(7, 2) : "FF";

            // Parse substrs.
            return new(
                byte.Parse(r, NumberStyles.HexNumber),
                byte.Parse(g, NumberStyles.HexNumber),
                byte.Parse(b, NumberStyles.HexNumber),
                byte.Parse(a, NumberStyles.HexNumber)
            );
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as @bool gdcolor.");
        }
    }
}