#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for 3x3 matrices.
/// </summary>
public struct Basis(Godot.Basis value) : ISerializer<Godot.Basis>
{
    /* Public properties. */
    public readonly string TypeCode => "f3x3";
    public Godot.Basis Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Basis(Basis value) => value.Value;
    public static implicit operator Basis(Godot.Basis value) => new(value);

    /* Public methods. */
    public readonly string Serialize() =>
           $"[{Value.X.X},{Value.X.Y},{Value.X.Z}]"
        + $",[{Value.Y.X},{Value.Y.Y},{Value.Y.Z}]"
        + $",[{Value.Z.X},{Value.Z.Y},{Value.Z.Z}]]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Basis.Identity;
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

        // Store parsed value.
        Value = new Godot.Basis(x, y, z);
    }

    public void Clear()
    {
        Value = Godot.Basis.Identity;
    }
}
#endif