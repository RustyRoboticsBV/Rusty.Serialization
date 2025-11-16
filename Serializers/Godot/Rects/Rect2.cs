#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for float rectangles.
/// </summary>
public struct Rect2(Godot.Rect2 value) : ISerializer<Godot.Rect2>
{
    /* Public properties. */
    public readonly string TypeCode => "r2f";
    public Godot.Rect2 Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Rect2(Rect2 value) => value.Value;
    public static implicit operator Rect2(Godot.Rect2 value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[[{Value.Position.X},{Value.Position.Y}],[{Value.Size.X},{Value.Size.Y}]]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = new();
            return;
        }

        // Split cells.
        List<string> parts = ListParser.Parse(text);

        // Parse cells.
        Vector2 position = new();
        if (parts.Count > 0)
            position.Deserialize(parts[0]);

        Vector2 size = new();
        if (parts.Count > 1)
            size.Deserialize(parts[1]);

        // Store parsed value.
        Value = new Godot.Rect2(position, size);
    }

    public void Clear()
    {
        Value = new();
    }
}
#endif