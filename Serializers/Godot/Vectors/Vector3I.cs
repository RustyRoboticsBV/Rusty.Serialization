#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for integer vectors of length 3.
/// </summary>
public struct Vector3I(Godot.Vector3I value) : ISerializer<Godot.Vector3I>
{
    /* Public properties. */
    public readonly string TypeCode => "i3";
    public Godot.Vector3I Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Vector3I(Vector3I value) => value.Value;
    public static implicit operator Vector3I(Godot.Vector3I value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y},{Value.Z}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Vector3I.Zero;
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

        // Store parsed value.
        Value = new Godot.Vector3I(x, y, z);
    }

    public void Clear()
    {
        Value = Godot.Vector3I.Zero;
    }
}
#endif