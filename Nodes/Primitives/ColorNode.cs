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
        return $"color: ({r},{g},{b},{a})";
    }

    public readonly string Serialize()
    {
        if (a == 255)
            return $"#{r:X2}{g:X2}{b:X2}";
        else
            return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    public static ColorNode Parse(string text)
    {
        // Remove whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Enforce leading hex sign.
            if (!trimmed.StartsWith('#'))
                throw new ArgumentException("Missing hex sign.");

            // Allow CSS convention.
            if (trimmed.Length == 4)
            {
                trimmed = trimmed[0]
                    + new string(trimmed[1], 2)
                    + new string(trimmed[2], 2)
                    + new string(trimmed[3], 2);
            }
            else if (trimmed.Length == 5)
            {
                trimmed = trimmed[0]
                    + new string(trimmed[1], 2)
                    + new string(trimmed[2], 2)
                    + new string(trimmed[3], 2)
                    + new string(trimmed[4], 2);
            }

            // Split into substrs.
            if (trimmed.Length != 7 && trimmed.Length != 9)
                throw new Exception("Bad length. Use #RGB, #RGBA, #RRGGBB or #RRGGBBAA.");
            string r = trimmed.Substring(1, 2);
            string g = trimmed.Substring(3, 2);
            string b = trimmed.Substring(5, 2);
            string a = trimmed.Length == 9 ? trimmed.Substring(7, 2) : "FF";

            // Parse substrs.
            try
            {
                return new(
                    byte.Parse(r, NumberStyles.HexNumber),
                    byte.Parse(g, NumberStyles.HexNumber),
                    byte.Parse(b, NumberStyles.HexNumber),
                    byte.Parse(a, NumberStyles.HexNumber)
                );
            }
            catch
            {
                throw new ArgumentException("Not a hexadecimal number.");
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a color:\n{ex.Message}");
        }
    }
}