using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization;

/// <summary>
/// An ushort serializer.
/// </summary>
public readonly struct UshortSerializer : ISerializer<ushort>
{
    /* Public methods. */
    public INode Serialize(ushort value, Registry context) => new IntNode(value);

    public ushort Deserialize(INode node, Registry context)
    {
        if (node is IntNode typed)
            return (ushort)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}