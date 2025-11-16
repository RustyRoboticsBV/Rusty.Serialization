#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for float vectors of length 2.
/// </summary>
public struct Vector2(Godot.Vector2 value) : ISerializer<Godot.Vector2>
{
    /* Public properties. */
    public readonly string TypeCode => "f2";
    public Godot.Vector2 Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Vector2(Vector2 value) => value.Value;
    public static implicit operator Vector2(Godot.Vector2 value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Vector2.Zero;
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

        // Store parsed value.
        Value = new Godot.Vector2(x, y);
    }

    public void Clear()
    {
        Value = Godot.Vector2.Zero;
    }
}
#endif