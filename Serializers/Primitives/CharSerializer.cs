using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A char serializer.
/// </summary>
public readonly struct CharSerializer : ISerializer<char>
{
    /* Public methods. */
    public INode Serialize(char value, Registry context) => new CharNode(value);

    public char Deserialize(INode node, Registry context)
    {
        if (node is CharNode charNode)
            return charNode.Value;
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}
