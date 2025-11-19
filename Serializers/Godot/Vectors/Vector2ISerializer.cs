#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.GodotEngine;

/// <summary>
/// A Vector2I serializer.
/// </summary>
public readonly struct Vector2ISerializer : ISerializer<Vector2I>
{
    /* Public methods. */
    public INode Serialize(Vector2I value, Registry context)
    {
        return new ListNode([new IntNode(value.X), new IntNode(value.Y)]);
    }

    public Vector2I Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 2)
                throw new ArgumentException("Cannot deserialize list node as Vector2 (wrong number of elements).");

            int x = 0;
            if (list.Elements[0] is IntNode xNode)
                x = (int)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector2 (x is not an int).");

            int y = 0;
            if (list.Elements[1] is IntNode yNode)
                y = (int)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize list node as Vector2 (y is not an int).");

            return new(x, y);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif