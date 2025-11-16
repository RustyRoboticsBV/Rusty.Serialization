#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for quaternions.
/// </summary>
public struct Quaternion(Godot.Quaternion value) : ISerializer<Godot.Quaternion>
{
    /* Public properties. */
    public readonly string TypeCode => "qt";
    public Godot.Quaternion Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Quaternion(Quaternion value) => value.Value;
    public static implicit operator Quaternion(Godot.Quaternion value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"[{Value.X},{Value.Y},{Value.Z},{Value.W}]";

    public void Deserialize(string text)
    {
        // Handle empty strings.
        if (string.IsNullOrWhiteSpace(text))
        {
            Value = Godot.Quaternion.Identity;
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

        Float w = new();
        if (parts.Count > 3)
            w.Deserialize(parts[3]);

        // Store parsed value.
        Value = new Godot.Quaternion(x, y, z, w);
    }

    public void Clear()
    {
        Value = Godot.Quaternion.Identity;
    }
}
#endif