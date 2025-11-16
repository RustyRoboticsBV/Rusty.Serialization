#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for planes.
/// </summary>
public struct Plane(Godot.Plane value) : ISerializer<Godot.Plane>
{
    /* Public properties. */
    public readonly string TypeCode => "pl";
    public Godot.Plane Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Plane(Plane value) => value.Value;
    public static implicit operator Plane(Godot.Plane value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y},{Value.Z},{Value.D}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Plane.PlaneXY;
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

        Float d = new();
        if (parts.Count > 3)
            d.Deserialize(parts[3]);

        // Store parsed value.
        Value = new Godot.Plane(x, y, z, d);
    }

    public void Clear()
    {
        Value = Godot.Plane.PlaneXY;
    }
}
#endif