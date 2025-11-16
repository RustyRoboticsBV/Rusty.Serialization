using System;
using System.Globalization;
using SysColor = System.Drawing.Color;
#if GODOT
using GdColor = Godot.Color;
#endif
#if UNITY_5_OR_NEWER
using UColor = UnityEngine.Color;
using UColor32 = UnityEngine.Color32;
#endif

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A color serializer node.
/// </summary>
public struct Color : INode
{
    /* Fields. */
    private readonly byte r;
    private readonly byte g;
    private readonly byte b;
    private readonly byte a;

    /* Constructors. */
    private Color(byte r, byte g, byte b, byte a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    /* Conversion operators. */
    public static implicit operator Color(SysColor value) => new(value.R, value.G, value.B, value.A);
#if GODOT
    public static implicit operator Color(GdColor value) => new((byte)value.R8, (byte)value.G8, (byte)value.B8, (byte)value.A8);
#endif
#if UNITY_5_OR_NEWER
    public static implicit operator Color(UColor32 value) => new(value.r, value.g, value.b, value.a);
    public static implicit operator Color(UColor value) => (UColor32)value;
#endif

    public static implicit operator SysColor(Color value) => SysColor.FromArgb(value.a, value.r, value.g, value.b);
#if GODOT
    public static implicit operator GdColor(Color node) => new(node.r / 255f, node.g / 255f, node.b / 255f, node.a / 255f);
#endif
#if UNITY_5_OR_NEWER
    public static implicit operator UColor32(Color value) => new(value.r, value.g, value.b, value.a);
    public static implicit operator UColor(Color value) => (UColor32)this;
#endif

    /* Public methods. */
    public readonly string Serialize()
    {
        if (a == 255)
            return $"#{r:X2}{g:X2}{b:X2}";
        else
            return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    public static Color Deserialize(string text)
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
            throw new ArgumentException($"Could not parse string '{text}' as a color.");
        }
    }
}