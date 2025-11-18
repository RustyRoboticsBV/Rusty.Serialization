#if UNITY_5_OR_NEWER
using System;
using UnityEngine;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Unity;

/// <summary>
/// A Color32 serializer.
/// </summary>
public readonly struct Color32Serializer : ISerializer<Color32>
{
    /* Public methods. */
    public INode Serialize(Color32 value, Registry context)
        => new ColorNode(value.r, value.g, value.b, value.a);

    public Color32 Deserialize(INode node, Registry context)
    {
        if (node is ColorNode color)
            return new(color.R, color.G, color.B, color.A);
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif