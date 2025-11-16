#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for 3D axis-aligned bounding boxes.
/// </summary>
public struct Aabb(Godot.Aabb value) : ISerializer<Godot.Aabb>
{
    /* Public properties. */
    public readonly string TypeCode => "aabb";
    public Godot.Aabb Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Aabb(Aabb value) => value.Value;
    public static implicit operator Aabb(Godot.Aabb value) => new(value);

    /* Public methods. */
    public readonly string Serialize() =>
          $"[[{Value.Position.X},{Value.Position.Y},{Value.Position.Z}]"
        + $",[{Value.Size.X},{Value.Size.Y},{Value.Size.Z}]]";

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
        Vector3 position = new();
        if (parts.Count > 0)
            position.Deserialize(parts[0]);

        Vector3 size = new();
        if (parts.Count > 1)
            size.Deserialize(parts[1]);

        // Store parsed value.
        Value = new Godot.Aabb(position, size);
    }

    public void Clear()
    {
        Value = new();
    }
}
#endif