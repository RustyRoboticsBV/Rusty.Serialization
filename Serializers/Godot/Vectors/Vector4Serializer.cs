#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A Vector4 serializer.
/// </summary>
public readonly struct Vector4Serializer : ISerializer<Vector4>
{
    /* Public methods. */
    public INode Serialize(Vector4 value, Registry context, bool addTypeLabel = false)
    {
        return new ListNode([new RealNode((decimal)value.X), new RealNode((decimal)value.Y),
            new RealNode((decimal)value.Z), new RealNode((decimal)value.W)]);
    }

    public Vector4 Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ListNode list)
        {
            if (list.Elements.Length != 4)
                throw new ArgumentException("Cannot deserialize list node as Vector4 (wrong number of elements).");

            float x = 0;
            if (list.Elements[0] is RealNode xNode)
                x = (float)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4 (x is not a float).");

            float y = 0;
            if (list.Elements[1] is RealNode yNode)
                y = (float)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4 (y is not a float).");

            float z = 0;
            if (list.Elements[2] is RealNode zNode)
                z = (float)zNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4 (z is not a float).");

            float w = 0;
            if (list.Elements[3] is RealNode wNode)
                w = (float)wNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector4 (w is not a float).");

            return new(x, y, z, w);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif