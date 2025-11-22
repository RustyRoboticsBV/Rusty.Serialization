using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A decimal serializer.
/// </summary>
public readonly struct DecimalSerializer : ISerializer<decimal>
{
    /* Public methods. */
    public INode Serialize(decimal value, Registry context, bool addTypeLabel = false) => new RealNode(value);

    public decimal Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is RealNode typed)
            return typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}