using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic dictionary converter.
    /// </summary>
    public abstract class GenericDictionaryConverter<DictionaryT, KeyT, ValueT> : ReferenceConverter<DictionaryT, DictNode>
        where DictionaryT : class, IDictionary<KeyT, ValueT>, new()
    {
        /* Protected methods. */
        protected sealed override DictNode ConvertRef(DictionaryT obj, IConverterScheme scheme)
        {
            Type keyType = typeof(KeyT);
            Type valueType = typeof(ValueT);

            // Convert the elements to nodes.
            List<KeyValuePair<INode, INode>> nodePairs = new();
            foreach (KeyValuePair<KeyT, ValueT> element in obj)
            {
                INode key = ConvertNested(keyType, element.Key, scheme);
                INode value = ConvertNested(valueType, element.Key, scheme);

                // Add pair.
                nodePairs.Add(new(key, value));
            }

            // Create the node.
            return new(nodePairs.ToArray());
        }

        protected sealed override DictionaryT DeconvertRef(DictNode node, IConverterScheme scheme)
        {
            DictionaryT obj = new();
            foreach (var pair in node.Pairs)
            {
                KeyT key = DeconvertNested<KeyT>(pair.Key, scheme);
                ValueT value = DeconvertNested<ValueT>(pair.Value, scheme);
                obj[key] = value;
            }
            return obj;
        }
    }
}