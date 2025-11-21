#if GODOT
using System;
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers.Gd;

/// <summary>
/// A StringName serializer.
/// </summary>
public readonly struct StringNameSerializer : ISerializer<StringName>
{
    /* Public methods. */
    public INode Serialize(StringName value, Registry context, bool addTypeLabel = false) => new StringNode(value);

    public StringName Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is StringNode str)
            return new(str.Value);
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
#endif