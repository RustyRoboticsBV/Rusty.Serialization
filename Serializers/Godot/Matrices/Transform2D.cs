#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for 2x3 matrices.
/// </summary>
public struct Transform2D(Godot.Transform2D value) : ISerializer<Godot.Transform2D>
{
    /* Public properties. */
    public readonly string TypeCode => "f2x3";
    public Godot.Transform2D Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Transform2D(Transform2D value) => value.Value;
    public static implicit operator Transform2D(Godot.Transform2D value) => new(value);

    /* Public methods. */
    public readonly string Serialize() =>
           $"[{Value.X.X},{Value.X.Y}]"
        + $",[{Value.Y.X},{Value.Y.Y}]"
        + $",[{Value.Origin.X},{Value.Origin.Y}]]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Transform2D.Identity;
            return;
        }

        // Split cells.
        List<string> parts = ListParser.Parse(text);

        // Parse cells.
        Vector2 x = new();
        if (parts.Count > 0)
            x.Deserialize(parts[0]);

        Vector2 y = new();
        if (parts.Count > 1)
            y.Deserialize(parts[1]);

        Vector2 origin = new();
        if (parts.Count > 2)
            origin.Deserialize(parts[2]);

        // Store parsed value.
        Value = new Godot.Transform2D(x, y, origin);
    }

    public void Clear()
    {
        Value = Godot.Transform2D.Identity;
    }
}
#endif