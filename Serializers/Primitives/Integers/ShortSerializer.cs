using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization;

/// <summary>
/// A short serializer.
/// </summary>
public readonly struct ShortSerializer : ISerializer<short>
{
    /* Public methods. */
    public INode Serialize(short value, Registry context) => new IntNode(value);

    public short Deserialize(INode node, Registry context)
    {
        if (node is IntNode typed)
            return (short)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}