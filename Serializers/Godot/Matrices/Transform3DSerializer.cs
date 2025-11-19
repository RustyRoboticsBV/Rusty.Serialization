#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.GodotEngine;

/// <summary>
/// A Transform3D serializer.
/// </summary>
public readonly struct Transform3DSerializer : ISerializer<Transform3D>
{
    /* Fields. */
    private readonly Vector3Serializer serializer;

    /* Public methods. */
    public INode Serialize(Transform3D value, Registry context)
    {
        return new ListNode([
            serializer.Serialize(value.Basis.X, context),
            serializer.Serialize(value.Basis.Y, context),
            serializer.Serialize(value.Basis.Z, context),
            serializer.Serialize(value.Origin, context)
        ]);
    }

    public Transform3D Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 4)
                throw new ArgumentException("Cannot deserialize list node as Transform3D (wrong number of elements).");

            Vector3 x = Vector3.Zero;
            if (list.Elements[0] is ListNode xNode)
                x = serializer.Deserialize(xNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Transform3D (x is not a list).");

            Vector3 y = Vector3.Zero;
            if (list.Elements[1] is ListNode yNode)
                y = serializer.Deserialize(yNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Transform3D (y is not a list).");

            Vector3 z = Vector3.Zero;
            if (list.Elements[2] is ListNode zNode)
                z = serializer.Deserialize(zNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Transform3D (z is not a list).");

            Vector3 origin = Vector3.Zero;
            if (list.Elements[3] is ListNode originNode)
                origin = serializer.Deserialize(originNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Transform3D (origin is not a list).");

            return new(x, y, z, origin);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif