#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for 4x4 matrices.
/// </summary>
public struct Projection(Godot.Projection value) : ISerializer<Godot.Projection>
{
    /* Public properties. */
    public readonly string TypeCode => "f4x4";
    public Godot.Projection Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Projection(Projection value) => value.Value;
    public static implicit operator Projection(Godot.Projection value) => new(value);
    public static implicit operator Projection(string text)
    {
        Projection result = new();
        result.Deserialize(text);
        return result;
    }

    /* Public methods. */
    public readonly string Serialize() =>
           $"[{Value.X.X},{Value.X.Y},{Value.X.Z},{Value.X.W}]"
        + $",[{Value.Y.X},{Value.Y.Y},{Value.Y.Z},{Value.Y.W}]"
        + $",[{Value.Z.X},{Value.Z.Y},{Value.Z.Z},{Value.Z.W}]"
        + $",[{Value.W.X},{Value.W.Y},{Value.W.Z},{Value.W.W}]]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Projection.Identity;
            return;
        }

        // Split cells.
        List<string> parts = ListParser.Parse(text);

        // Parse cells.
        Vector4 x = new();
        if (parts.Count > 0)
            x.Deserialize(parts[0]);

        Vector4 y = new();
        if (parts.Count > 1)
            y.Deserialize(parts[1]);

        Vector4 z = new();
        if (parts.Count > 2)
            z.Deserialize(parts[2]);

        Vector4 w = new();
        if (parts.Count > 3)
            w.Deserialize(parts[3]);

        System.Console.WriteLine(parts[0]);
        System.Console.WriteLine(parts[1]);
        System.Console.WriteLine(parts[2]);
        System.Console.WriteLine(parts[3]);

        // Store parsed value.
        Value = new Godot.Projection(x, y, z, w);
    }

    public void Clear()
    {
        Value = Godot.Projection.Identity;
    }
}
#endif