using System;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization;

/// <summary>
/// A dictionary serializer.
/// </summary>
public readonly struct DictionarySerializer<KeyT, ValueT> : ISerializer<Dictionary<KeyT, ValueT>>
{
    /* Public methods. */
    public INode Serialize(Dictionary<KeyT, ValueT> value, Registry context)
    {
        // Handle null.
        if (value == null)
            return new NullNode();

        // Serialize key-value pairs.
        ISerializer keySerializer = context.GetSerializer(typeof(KeyT));
        ISerializer valueSerializer = context.GetSerializer(typeof(ValueT));

        var pairs = new KeyValuePair<INode, INode>[value.Count];
        int i = 0;
        foreach (var pair in value)
        {
            pairs[i] = new KeyValuePair<INode, INode>(
                keySerializer.Serialize(pair.Key, context),
                valueSerializer.Serialize(pair.Value, context)
            );
            i++;
        }

        return new DictNode(pairs);
    }

    public Dictionary<KeyT, ValueT> Deserialize(INode node, Registry context)
    {
        if (node is NullNode @null)
            return null;
        else if (node is DictNode dict)
        {
            // Deserialize elements.
            ISerializer keySerializer = context.GetSerializer(typeof(KeyT));
            ISerializer valueSerializer = context.GetSerializer(typeof(ValueT));

            Dictionary<KeyT, ValueT> result = new();
            foreach (KeyValuePair<INode, INode> pair in dict.Pairs)
            {
                KeyT key = (KeyT)keySerializer.Deserialize(pair.Key, context);
                ValueT value = (ValueT)valueSerializer.Deserialize(pair.Value, context);
                result.Add(key, value);
            }

            return result;
        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }
}