using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A null serializer.
/// </summary>
public readonly struct NullSerializer : ISerializer<object>
{
    /* Public methods. */
    public INode Serialize(object value, Registry context, bool addTypeLabel = false) => new NullNode();

    public object Deserialize(INode node, Registry context)
    {
        if (node is TypeNode  || node is NullNode)
            return null;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
