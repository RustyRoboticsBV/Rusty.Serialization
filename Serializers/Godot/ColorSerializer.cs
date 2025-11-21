#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A Color serializer.
/// </summary>
public readonly struct ColorSerializer : ISerializer<Color>
{
    /* Public methods. */
    public INode Serialize(Color value, Registry context, bool addTypeLabel = false)
        => new ColorNode((byte)value.R8, (byte)value.G8, (byte)value.B8, (byte)value.A8);

    public Color Deserialize(INode node, Registry context)
    {
        if (node is ColorNode color)
            return new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif