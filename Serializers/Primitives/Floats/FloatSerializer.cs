using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A float serializer.
/// </summary>
public readonly struct FloatSerializer : ISerializer<float>
{
    /* Public methods. */
    public INode Serialize(float value, Registry context) => new FloatNode((decimal)value);

    public float Deserialize(INode node, Registry context)
    {
        if (node is FloatNode typed)
            return (float)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}