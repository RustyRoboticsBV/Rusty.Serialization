#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.GodotEngine;

/// <summary>
/// A Quaternion serializer.
/// </summary>
public readonly struct QuaternionSerializer : ISerializer<Quaternion>
{
    /* Public methods. */
    public INode Serialize(Quaternion value, Registry context)
    {
        return new ListNode([new FloatNode((decimal)value.X), new FloatNode((decimal)value.Y),
            new FloatNode((decimal)value.Z), new FloatNode((decimal)value.W)]);
    }

    public Quaternion Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 4)
                throw new ArgumentException("Cannot deserialize list node as Quaternion (wrong number of elements).");

            float x = 0;
            if (list.Elements[0] is FloatNode xNode)
                x = (float)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Quaternion (x is not a float).");

            float y = 0;
            if (list.Elements[1] is FloatNode yNode)
                y = (float)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Quaternion (y is not a float).");

            float z = 0;
            if (list.Elements[2] is FloatNode zNode)
                z = (float)zNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Quaternion (z is not a float).");

            float w = 0;
            if (list.Elements[3] is FloatNode wNode)
                w = (float)wNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Quaternion (w is not a float).");

            return new(x, y, z, w);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif