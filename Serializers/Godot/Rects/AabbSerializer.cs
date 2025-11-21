#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A Aabb serializer.
/// </summary>
public readonly struct AabbSerializer : ISerializer<Aabb>
{
    /* Fields. */
    private readonly Vector3Serializer serializer;

    /* Public methods. */
    public INode Serialize(Aabb value, Registry context, bool addTypeLabel = false)
    {
        INode position = serializer.Serialize(value.Position, context);
        INode size = serializer.Serialize(value.Size, context);
        return new ListNode([position, size]);
    }

    public Aabb Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ListNode list)
        {
            if (list.Elements.Length != 2)
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Aabb (wrong number of elements).");

            Vector3 position = Vector3.Zero;
            if (list.Elements[0] is ListNode positionNode)
                position = serializer.Deserialize(positionNode, context);
            else
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Aabb (position is not a list).");

            Vector3 size = Vector3.Zero;
            if (list.Elements[1] is ListNode sizeNode)
                size = serializer.Deserialize(sizeNode, context);
            else
                throw new ArgumentException($"'{GetType()}' cannot deserialize list node as Aabb (size is not a list).");

            return new(position, size);
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif