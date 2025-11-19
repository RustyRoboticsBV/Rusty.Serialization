#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.GodotEngine;

/// <summary>
/// A Plane serializer.
/// </summary>
public readonly struct PlaneSerializer : ISerializer<Plane>
{
    /* Public methods. */
    public INode Serialize(Plane value, Registry context)
    {
        ListNode origin = new ListNode([
            new FloatNode((decimal)value.X),
            new FloatNode((decimal)value.Y),
            new FloatNode((decimal)value.Z)]
        );
        FloatNode distance = new FloatNode((decimal)value.D);
        return new ListNode([origin, distance]);
    }

    public Plane Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 2)
                throw new ArgumentException("Cannot deserialize array node as Plane (wrong number of elements).");

            Vector3 normal = Vector3.Zero;
            if (list.Elements[0] is ListNode positionNode)
                normal = new Vector3Serializer().Deserialize(positionNode, context);
            else
                throw new ArgumentException("Cannot deserialize array node as Plane (normal is not @bool list).");

            float distance = 0f;
            if (list.Elements[1] is FloatNode distanceNode)
                distance = (float)distanceNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Plane (char is not @bool float).");

            return new(normal.X, normal.Y, normal.Z, distance);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif