#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A Vector3 serializer.
/// </summary>
public readonly struct Vector3Serializer : ISerializer<Vector3>
{
    /* Public methods. */
    public INode Serialize(Vector3 value, Registry context, bool addTypeLabel = false)
    {
        return new ListNode([new FloatNode((decimal)value.X), new FloatNode((decimal)value.Y), new FloatNode((decimal)value.Z)]);
    }

    public Vector3 Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ListNode list)
        {
            if (list.Elements.Length != 3)
                throw new ArgumentException("Cannot deserialize list node as Vector3 (wrong number of elements).");

            float x = 0;
            if (list.Elements[0] is FloatNode xNode)
                x = (float)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector3 (x is not a float).");

            float y = 0;
            if (list.Elements[1] is FloatNode yNode)
                y = (float)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector3 (y is not a float).");

            float z = 0;
            if (list.Elements[2] is FloatNode zNode)
                z = (float)zNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector3 (z is not a float).");

            return new(x, y, z);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif