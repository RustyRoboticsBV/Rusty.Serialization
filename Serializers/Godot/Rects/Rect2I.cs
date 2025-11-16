#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for integer rectangles.
/// </summary>
public struct Rect2I(Godot.Rect2I value) : ISerializer<Godot.Rect2I>
{
    /* Public properties. */
    public readonly string TypeCode => "r2i";
    public Godot.Rect2I Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Rect2I(Rect2I value) => value.Value;
    public static implicit operator Rect2I(Godot.Rect2I value) => new(value);

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
        Vector2I position = new();
        if (parts.Count > 0)
            position.Deserialize(parts[0]);

        Vector2I size = new();
        if (parts.Count > 1)
            size.Deserialize(parts[1]);

        // Store parsed value.
        Value = new Godot.Rect2I(position, size);
    }

    public void Clear()
    {
        Value = new();
    }
}
#endif