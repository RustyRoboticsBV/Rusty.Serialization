using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A string serializer.
/// </summary>
public readonly struct StringSerializer : ISerializer<string>
{
    /* Public methods. */
    public INode Serialize(string value, Registry context)
    {
        if (value == null)
            return new NullNode();
        return new StringNode(value);
    }

    public string Deserialize(INode node, Registry context)
    {
        if (node is NullNode)
            return null;
        if (node is StringNode stringNode)
            return stringNode.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
