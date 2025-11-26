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
        Type keyType = obj.GetType().GetGenericArguments()[0];
        Type valueType = obj.GetType().GetGenericArguments()[1];

        // Convert the elements to nodes.
        List<KeyValuePair<INode, INode>> nodePairs = new();
        foreach (KeyValuePair<KeyT, ValueT> element in obj)
        {
            // Convert key.
            Type elementKeyType = element.Key.GetType();
            INode key = ConvertElement(element.Key, context);
            if (elementKeyType != keyType)
                key = new TypeNode(context.GetTypeName(elementKeyType), key);

            // Convert value.
            Type elementValueType = element.Value.GetType();
            INode value = ConvertElement(element.Value, context);
            if (elementValueType != valueType)
                value = new TypeNode(context.GetTypeName(elementValueType), value);

            // Add pair.
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