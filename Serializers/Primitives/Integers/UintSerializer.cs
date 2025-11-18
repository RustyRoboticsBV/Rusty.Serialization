using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization;

/// <summary>
/// An uint serializer.
/// </summary>
public readonly struct UintSerializer : ISerializer<uint>
{
    /* Public methods. */
    public INode Serialize(uint value, Registry context) => new IntNode(value);

    public uint Deserialize(INode node, Registry context)
    {
        if (node is IntNode typed)
            return (uint)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}