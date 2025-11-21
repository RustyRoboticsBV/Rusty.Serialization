using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A double serializer.
/// </summary>
public readonly struct DoubleSerializer : ISerializer<double>
{
    /* Public methods. */
    public INode Serialize(double value, Registry context, bool addTypeLabel = false) => new FloatNode((decimal)value);

    public double Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is FloatNode typed)
            return (double)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}