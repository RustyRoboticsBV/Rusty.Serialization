using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization;

/// <summary>
/// An sbyte serializer.
/// </summary>
public readonly struct SbyteSerializer : ISerializer<sbyte>
{
    /* Public methods. */
    public INode Serialize(sbyte value, Registry context) => new IntNode(value);

    public sbyte Deserialize(INode node, Registry context)
    {
        if (node is IntNode typed)
            return (sbyte)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}