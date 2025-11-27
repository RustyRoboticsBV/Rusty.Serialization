using System;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic dictionary converter.
    /// </summary>
    public abstract class GenericDictionaryConverter<DictionaryT, KeyT, ValueT> : ReferenceConverter<DictionaryT, DictNode>
        where DictionaryT : class, IDictionary<KeyT, ValueT>, new()
    {
        /* Protected methods. */
        protected sealed override DictNode Convert(DictionaryT obj, Context context)
        {
            Type keyType = typeof(KeyT);
            Type valueType = typeof(ValueT);

            // Convert the elements to nodes.
            List<KeyValuePair<INode, INode>> nodePairs = new();
            foreach (KeyValuePair<KeyT, ValueT> element in obj)
            {
                INode key = ConvertElement(keyType, element.Key, context);
                INode value = ConvertElement(valueType, element.Key, context);

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
    }
}