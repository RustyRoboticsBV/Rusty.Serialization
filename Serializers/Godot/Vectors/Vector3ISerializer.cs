#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.GodotEngine;

/// <summary>
/// A Vector3I serializer.
/// </summary>
public readonly struct Vector3ISerializer : ISerializer<Vector3I>
{
    /* Public methods. */
    public INode Serialize(Vector3I value, Registry context)
    {
        return new ListNode([new IntNode(value.X), new IntNode(value.Y), new IntNode(value.Z)]);
    }

    public Vector3I Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 3)
                throw new ArgumentException("Cannot deserialize array node as Vector3I (wrong number of elements).");

            int x = 0;
            if (list.Elements[0] is IntNode xNode)
                x = (int)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector3I (x is not an int).");

            int y = 0;
            if (list.Elements[1] is IntNode yNode)
                y = (int)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector3I (y is not an int).");

            int z = 0;
            if (list.Elements[2] is IntNode zNode)
                z = (int)zNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector3I (z is not an int).");

            return new(x, y, z);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif