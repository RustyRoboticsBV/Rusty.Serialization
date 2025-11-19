using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A long serializer.
/// </summary>
public readonly struct LongSerializer : ISerializer<long>
{
    /* Public methods. */
    public INode Serialize(long value, Registry context) => new IntNode(value);

    public long Deserialize(INode node, Registry context)
    {
        if (node is IntNode typed)
            return (long)typed.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}