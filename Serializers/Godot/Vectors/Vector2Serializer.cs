#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.GodotEngine;

/// <summary>
/// A Vector2 serializer.
/// </summary>
public readonly struct Vector2Serializer : ISerializer<Vector2>
{
    /* Public methods. */
    public INode Serialize(Vector2 value, Registry context)
    {
        return new ListNode([new FloatNode((decimal)value.X), new FloatNode((decimal)value.Y)]);
    }

    public Vector2 Deserialize(INode node, Registry context)
    {
        if (node is ListNode list)
        {
            if (list.Elements.Length != 2)
                throw new ArgumentException("Cannot deserialize array node as Vector2 (wrong number of elements).");

            float x = 0f;
            if (list.Elements[0] is FloatNode xNode)
                x = (float)xNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector2 (x is not a float).");

            float y = 0f;
            if (list.Elements[1] is FloatNode yNode)
                y = (float)yNode.Value;
            else
                throw new ArgumentException("Cannot deserialize array node as Vector2 (y is not a float).");

            return new(x, y);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif