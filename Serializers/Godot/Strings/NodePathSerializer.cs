#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.GodotEngine;

/// <summary>
/// A NodePath serializer.
/// </summary>
public readonly struct NodePathSerializer : ISerializer<NodePath>
{
    /* Public methods. */
    public INode Serialize(NodePath value, Registry context) => new StringNode(value);

    public NodePath Deserialize(INode node, Registry context)
    {
        if (node is StringNode str)
            return new(str.Value);
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif