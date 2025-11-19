#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.GodotEngine;

/// <summary>
/// A Rect2I serializer.
/// </summary>
public readonly struct Rect2ISerializer : ISerializer<Rect2I>
{
    /* Fields. */
    private readonly Vector2ISerializer serializer;

    /* Public methods. */
    public INode Serialize(Rect2I value, Registry context)
    {
        INode position = serializer.Serialize(value.Position, context);
        INode size = serializer.Serialize(value.Size, context);
        return new ListNode([position, size]);
    }

    public Rect2I Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 2)
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Rect2I (wrong number of elements).");

            Vector2I position = Vector2I.Zero;
            if (list.Elements[0] is ListNode positionNode)
                position = serializer.Deserialize(positionNode, context);
            else
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Rect2I (position is not a list).");

            Vector2I size = Vector2I.Zero;
            if (list.Elements[1] is ListNode sizeNode)
                size = serializer.Deserialize(sizeNode, context);
            else
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Rect2I (size is not a list).");

            return new(position, size);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif