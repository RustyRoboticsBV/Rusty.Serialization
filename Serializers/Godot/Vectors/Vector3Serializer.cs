#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.GodotEngine;

/// <summary>
/// A Vector3 serializer.
/// </summary>
public readonly struct Vector3Serializer : ISerializer<Vector3>
{
    /* Public methods. */
    public INode Serialize(Vector3 value, Registry context)
    {
        return new ListNode([new FloatNode((decimal)value.X), new FloatNode((decimal)value.Y), new FloatNode((decimal)value.Z)]);
    }

    public Vector3 Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 3)
                throw new ArgumentException("Cannot deserialize array node as Vector3 (wrong number of elements).");

            float x = 0;
            if (list.Elements[0] is FloatNode xNode)
                x = (float)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector3 (x is not @bool float).");

            float y = 0;
            if (list.Elements[1] is FloatNode yNode)
                y = (float)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector3 (y is not @bool float).");

            float z = 0;
            if (list.Elements[2] is FloatNode zNode)
                z = (float)zNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector3 (z is not @bool float).");

            return new(x, y, z);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif