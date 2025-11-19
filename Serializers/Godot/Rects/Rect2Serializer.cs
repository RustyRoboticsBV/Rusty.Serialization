#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.GodotEngine;

/// <summary>
/// A Rect2 serializer.
/// </summary>
public readonly struct Rect2Serializer : ISerializer<Rect2>
{
    /* Fields. */
    private readonly Vector2Serializer serializer;

    /* Public methods. */
    public INode Serialize(Rect2 value, Registry context)
    {
        INode position = serializer.Serialize(value.Position, context);
        INode size = serializer.Serialize(value.Size, context);
        return new ListNode([position, size]);
    }

    public Rect2 Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 2)
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Rect2 (wrong number of elements).");

            Vector2 position = Vector2.Zero;
            if (list.Elements[0] is ListNode positionNode)
                position = serializer.Deserialize(positionNode, context);
            else
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Rect2 (position is not a list).");

            Vector2 size = Vector2.Zero;
            if (list.Elements[1] is ListNode sizeNode)
                size = serializer.Deserialize(sizeNode, context);
            else
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Rect2 (size is not a list).");

            return new(position, size);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif