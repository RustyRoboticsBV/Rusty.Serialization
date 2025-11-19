using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// An ulong serializer.
/// </summary>
public readonly struct UlongSerializer : ISerializer<ulong>
{
    /* Public methods. */
    public INode Serialize(ulong value, Registry context) => new IntNode(value);

    public ulong Deserialize(INode node, Registry context)
    {
        if (node is IntNode typed)
            return (ulong)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}