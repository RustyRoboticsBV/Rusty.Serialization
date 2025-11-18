using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization;

/// <summary>
/// A decimal serializer.
/// </summary>
public readonly struct DecimalSerializer : ISerializer<decimal>
{
    /* Public methods. */
    public INode Serialize(decimal value, Registry context) => new FloatNode(value);

    public decimal Deserialize(INode node, Registry context)
    {
        if (node is FloatNode typed)
            return typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}