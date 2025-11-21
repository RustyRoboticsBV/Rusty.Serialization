#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A Basis serializer.
/// </summary>
public readonly struct BasisSerializer : ISerializer<Basis>
{
    /* Fields. */
    private readonly Vector3Serializer serializer;

    /* Public methods. */
    public INode Serialize(Basis value, Registry context, bool addTypeLabel = false)
    {
        return new ListNode([
            serializer.Serialize(value.X, context),
            serializer.Serialize(value.Y, context),
            serializer.Serialize(value.Z, context)
        ]);
    }

    public Basis Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ListNode list)
        {
            if (list.Elements.Length != 3)
                throw new ArgumentException("Cannot deserialize list node as Basis (wrong number of elements).");

            Vector3 x = Vector3.Zero;
            if (list.Elements[0] is ListNode xNode)
                x = serializer.Deserialize(xNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Basis (x is not a list).");

            Vector3 y = Vector3.Zero;
            if (list.Elements[1] is ListNode yNode)
                y = serializer.Deserialize(yNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Basis (y is not a list).");

            Vector3 z = Vector3.Zero;
            if (list.Elements[2] is ListNode zNode)
                z = serializer.Deserialize(zNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Basis (z is not a list).");

            return new(x, y, z);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif