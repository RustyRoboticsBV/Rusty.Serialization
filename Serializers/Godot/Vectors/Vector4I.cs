#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for integer vectors of length 4.
/// </summary>
public struct Vector4I(Godot.Vector4I value) : ISerializer<Godot.Vector4I>
{
    /* Public properties. */
    public readonly string TypeCode => "i4";
    public Godot.Vector4I Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Vector4I(Vector4I value) => value.Value;
    public static implicit operator Vector4I(Godot.Vector4I value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y},{Value.Z},{Value.W}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Vector4I.Zero;
            return;
        }

        // Split cells.
        List<string> parts = ListParser.Parse(text);

        // Parse cells.
        Int x = new();
        if (parts.Count > 0)
            x.Deserialize(parts[0]);

        Int y = new();
        if (parts.Count > 1)
            y.Deserialize(parts[1]);

        Int z = new();
        if (parts.Count > 2)
            z.Deserialize(parts[2]);

        Int w = new();
        if (parts.Count > 3)
            w.Deserialize(parts[3]);

        // Store parsed value.
        Value = new Godot.Vector4I(x, y, z, w);
    }

    public void Clear()
    {
        Value = Godot.Vector4I.Zero;
    }
}
#endif