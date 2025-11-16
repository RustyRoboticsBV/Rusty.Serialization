#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for float vectors of length 3.
/// </summary>
public struct Vector3(Godot.Vector3 value) : ISerializer<Godot.Vector3>
{
    /* Public properties. */
    public readonly string TypeCode => "f3";
    public Godot.Vector3 Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Vector3(Vector3 value) => value.Value;
    public static implicit operator Vector3(Godot.Vector3 value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y},{Value.Z}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Vector3.Zero;
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

        // Store parsed value.
        Value = new Godot.Vector3(x, y, z);
    }

    public void Clear()
    {
        Value = Godot.Vector3.Zero;
    }
}
#endif