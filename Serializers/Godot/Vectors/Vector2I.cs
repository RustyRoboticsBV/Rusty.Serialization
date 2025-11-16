#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for integer vectors of length 2.
/// </summary>
public struct Vector2I(Godot.Vector2I value) : ISerializer<Godot.Vector2I>
{
    /* Public properties. */
    public readonly string TypeCode => "i3";
    public Godot.Vector2I Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Vector2I(Vector2I value) => value.Value;
    public static implicit operator Vector2I(Godot.Vector2I value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Vector2I.Zero;
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

        // Store parsed value.
        Value = new Godot.Vector2I(x, y);
    }

    public void Clear()
    {
        Value = Godot.Vector2I.Zero;
    }
}
#endif