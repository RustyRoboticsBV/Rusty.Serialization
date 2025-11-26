using System;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A key-value pair converter.
/// </summary>
public sealed class KeyValuePairConverter<KeyT, ValueT> : ValueConverter<KeyValuePair<KeyT, ValueT>, DictNode>
{
    /* Protected methods. */
    protected sealed override DictNode Convert(KeyValuePair<KeyT, ValueT> obj, Context context)
    {
        INode key = ConvertElement(typeof(KeyT), obj.Key, context);
        INode value = ConvertElement(typeof(ValueT), obj.Value, context);
        return new([new(key, value)]);
    }

    protected sealed override KeyValuePair<KeyT, ValueT> Deconvert(DictNode node, Context context)
    {
        if (node.Pairs.Length != 1)
            throw new ArgumentException("Cannot deserialize dict node with length that isn't 1.");
        KeyT key = DeconvertElement<KeyT>(node.Pairs[0].Key, context);
        ValueT value = DeconvertElement<ValueT>(node.Pairs[0].Value, context);
        return new(key, value);
    }
}