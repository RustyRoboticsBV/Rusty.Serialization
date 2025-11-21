using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A string serializer.
/// </summary>
public readonly struct EnumSerializer<T> : ISerializer<T>
    where T : Enum
{
    /* Public methods. */
    public INode Serialize(T value, Registry context, bool addTypeLabel = false)
    {
        return new IntNode(Convert.ToDecimal(value));
    }

    public T Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is IntNode intNode)
        {
            if (intNode.Value < 0)
                return (T)Enum.ToObject(typeof(T), (long)intNode.Value);
            else
                return (T)Enum.ToObject(typeof(T), (ulong)intNode.Value);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}