using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization;

/// <summary>
/// An int serializer.
/// </summary>
public readonly struct IntSerializer : ISerializer<int>
{
    /* Public methods. */
    public INode Serialize(int value, Registry context) => new IntNode(value);

    public int Deserialize(INode node, Registry context)
    {
        if (node is IntNode typed)
            return (int)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}