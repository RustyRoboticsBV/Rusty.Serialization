using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A byte serializer.
/// </summary>
public readonly struct ByteSerializer : ISerializer<byte>
{
    /* Public methods. */
    public INode Serialize(byte value, Registry context, bool addTypeLabel = false) => new IntNode(value);

    public byte Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is IntNode typed)
            return (byte)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}