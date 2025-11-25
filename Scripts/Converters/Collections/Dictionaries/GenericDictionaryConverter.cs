using System;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic dictionary converter.
/// </summary>
public abstract class GenericDictionaryConverter<DictionaryT, KeyT, ValueT> : ReferenceConverter<DictionaryT, DictNode>
    where DictionaryT : class, IDictionary<KeyT, ValueT>, new()
{
    /* Protected methods. */
    protected sealed override DictNode Convert(DictionaryT obj, Context context)
    {
        // Convert the elements to nodes.
        List<KeyValuePair<INode, INode>> nodePairs = new();
        foreach (KeyValuePair<KeyT, ValueT> element in obj)
        {
            INode key = ConvertElement(element.Key, context);
            INode value = ConvertElement(element.Value, context);
            nodePairs.Add(new(key, value));
        }

        // Create the node.
        return new(nodePairs.ToArray());
    }

    protected sealed override DictionaryT Deconvert(DictNode node, Context context)
    {
        DictionaryT obj = new();
        foreach (var pair in node.Pairs)
        {
            KeyT key = DeconvertElement<KeyT>(pair.Key, context);
            ValueT value = DeconvertElement<ValueT>(pair.Value, context);
            obj[key] = value;
        }
        return obj;
    }

    /* Private methods. */
    /// <summary>
    /// Convert an object to an INode.
    /// </summary>
    private INode ConvertElement<T>(T obj, Context context)
    {
        return context.GetConverter(obj.GetType()).Convert(obj, context);
    }

    /// <summary>
    /// Deconvert an INode into an object.
    /// </summary>
    private T DeconvertElement<T>(INode node, Context context)
    {
        Type targetType = ((IConverter)this).TargetType;
        return (T)context.GetConverter(targetType).Deconvert(node, context);
    }
}