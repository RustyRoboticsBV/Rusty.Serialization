#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for float vectors of length 4.
/// </summary>
public struct Vector4(Godot.Vector4 value) : ISerializer<Godot.Vector4>
{
    /* Public properties. */
    public readonly string TypeCode => "f4";
    public Godot.Vector4 Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Vector4(Vector4 value) => value.Value;
    public static implicit operator Vector4(Godot.Vector4 value) => new(value);
    public static implicit operator Vector4(string text)
    {
        Vector4 result = new();
        result.Deserialize(text);
        return result;
    }

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y},{Value.Z},{Value.W}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Vector4.Zero;
            return;
        }

        // Split cells.
        List<string> parts = ListParser.Parse(text);

        // Parse cells.
        Float x = new();
        if (parts.Count > 0)
            x.Deserialize(parts[0]);

        Float y = new();
        if (parts.Count > 1)
            y.Deserialize(parts[1]);

        Float z = new();
        if (parts.Count > 2)
            z.Deserialize(parts[2]);

        Float w = new();
        if (parts.Count > 3)
            w.Deserialize(parts[3]);

        // Store parsed value.
        Value = new Godot.Vector4(x, y, z, w);
    }

    public void Clear()
    {
        Value = Godot.Vector4.Zero;
    }
}
#endif