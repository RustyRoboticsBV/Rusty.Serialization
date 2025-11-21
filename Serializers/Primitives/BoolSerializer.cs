using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A bool serializer.
/// </summary>
public readonly struct BoolSerializer : ISerializer<bool>
{
    /* Public methods. */
    public INode Serialize(bool value, Registry context, bool addTypeLabel = false) => new BoolNode(value);

    public bool Deserialize(INode node, Registry context)
    {
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is BoolNode boolNode)
            return boolNode.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
