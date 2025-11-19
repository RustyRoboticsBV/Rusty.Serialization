#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.GodotEngine;

/// <summary>
/// A Transform2D serializer.
/// </summary>
public readonly struct Transform2DSerializer : ISerializer<Transform2D>
{
    /* Fields. */
    private readonly Vector2Serializer serializer;

    /* Public methods. */
    public INode Serialize(Transform2D value, Registry context)
    {
        return new ListNode([
            serializer.Serialize(value.X, context),
            serializer.Serialize(value.Y, context),
            serializer.Serialize(value.Origin, context)
        ]);
    }

    public Transform2D Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 3)
                throw new ArgumentException("Cannot deserialize list node as Transform2D (wrong number of elements).");

            Vector2 x = Vector2.Zero;
            if (list.Elements[0] is ListNode xNode)
                x = serializer.Deserialize(xNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Transform2D (x is not a list).");

            Vector2 y = Vector2.Zero;
            if (list.Elements[1] is ListNode yNode)
                y = serializer.Deserialize(yNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Transform2D (y is not a list).");

            Vector2 origin = Vector2.Zero;
            if (list.Elements[2] is ListNode originNode)
                origin = serializer.Deserialize(originNode, context);
            else
                throw new ArgumentException("Cannot deserialize list node as Transform2D (origin is not a list).");

            return new(x, y, origin);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif