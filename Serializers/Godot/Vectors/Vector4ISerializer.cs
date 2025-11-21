#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A Vector4I serializer.
/// </summary>
public readonly struct Vector4ISerializer : ISerializer<Vector4I>
{
    /* Public methods. */
    public INode Serialize(Vector4I value, Registry context, bool addTypeLabel = false)
    {
        return new ListNode([new IntNode(value.X), new IntNode(value.Y), new IntNode(value.Z), new IntNode(value.W)]);
    }

    public Vector4I Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ListNode list)
        {
            if (list.Elements.Length != 4)
                throw new ArgumentException("Cannot deserialize list node as Vector4I (wrong number of elements).");

            int x = 0;
            if (list.Elements[0] is IntNode xNode)
                x = (int)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4I (x is not an int).");

            int y = 0;
            if (list.Elements[1] is IntNode yNode)
                y = (int)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4I (y is not an int).");

            int z = 0;
            if (list.Elements[2] is IntNode zNode)
                z = (int)zNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4I (z is not an int).");

            int w = 0;
            if (list.Elements[3] is IntNode wNode)
                w = (int)wNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4I (w is not an int).");

            return new(x, y, z, w);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif