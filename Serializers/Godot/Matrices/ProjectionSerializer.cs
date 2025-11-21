#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A Projection serializer.
/// </summary>
public readonly struct ProjectionSerializer : ISerializer<Projection>
{
    /* Fields. */
    private readonly Vector4Serializer serializer;

    /* Public methods. */
    public INode Serialize(Projection value, Registry context, bool addTypeLabel = false)
    {
        return new ListNode([
            serializer.Serialize(value.X, context),
            serializer.Serialize(value.Y, context),
            serializer.Serialize(value.Z, context),
            serializer.Serialize(value.W, context)
        ]);
    }

    public Projection Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ListNode list)
        {
            if (list.Elements.Length != 4)
                throw new ArgumentException("Cannot deserialize list node as Projection (wrong number of elements).");

            Vector4 x = Vector4.Zero;
            if (list.Elements[0] is ListNode xNode)
                x = serializer.Deserialize(xNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Projection (x is not a list).");

            Vector4 y = Vector4.Zero;
            if (list.Elements[1] is ListNode yNode)
                y = serializer.Deserialize(yNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Projection (y is not a list).");

            Vector4 z = Vector4.Zero;
            if (list.Elements[2] is ListNode zNode)
                z = serializer.Deserialize(zNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Projection (z is not a list).");

            Vector4 w = Vector4.Zero;
            if (list.Elements[3] is ListNode wNode)
                w = serializer.Deserialize(wNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Projection (w is not a list).");

            return new(x, y, z, w);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif