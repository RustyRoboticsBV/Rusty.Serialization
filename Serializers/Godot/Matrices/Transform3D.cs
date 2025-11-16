#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for 3x4 matrices.
/// </summary>
public struct Transform3D(Godot.Transform3D value) : ISerializer<Godot.Transform3D>
{
    /* Public properties. */
    public readonly string TypeCode => "f3x4";
    public Godot.Transform3D Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Transform3D(Transform3D value) => value.Value;
    public static implicit operator Transform3D(Godot.Transform3D value) => new(value);

    /* Public methods. */
    public readonly string Serialize() =>
           $"[{Value.Basis.X.X},{Value.Basis.X.Y},{Value.Basis.X.Z}]"
        + $",[{Value.Basis.Y.X},{Value.Basis.Y.Y},{Value.Basis.Y.Z}]"
        + $",[{Value.Basis.Z.X},{Value.Basis.Z.Y},{Value.Basis.Z.Z}]"
        + $",[{Value.Origin.X},{Value.Origin.Y},{Value.Origin.Z}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Transform3D.Identity;
            return;
        }

        // Split cells.
        List<string> parts = ListParser.Parse(text);

        // Parse cells.
        Vector3 x = new();
        if (parts.Count > 0)
            x.Deserialize(parts[0]);

        Vector3 y = new();
        if (parts.Count > 1)
            y.Deserialize(parts[1]);

        Vector3 z = new();
        if (parts.Count > 2)
            z.Deserialize(parts[2]);

        Vector3 origin = new();
        if (parts.Count > 3)
            origin.Deserialize(parts[3]);

        // Store parsed value.
        Value = new Godot.Transform3D(new(x, y, z), origin);
    }

    public void Clear()
    {
        Value = Godot.Transform3D.Identity;
    }
}
#endif