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
            // Create new node.
            DictNode node = new(obj.Count);

            // Convert the elements to nodes.
            int index = 0;
            foreach (KeyValuePair<KeyT, ValueT> pair in obj)
            {
                var newPair = ConvertPair(pair, scheme);
                newPair.Key.Parent = node;
                newPair.Value.Parent = node;
                node.Pairs[index] = newPair;
                index++;
            }

            // Create the node.
            return node;
        }

        protected sealed override DictionaryT DeconvertRef(DictNode node, IConverterScheme scheme)
        {
            DictionaryT obj = new();
            foreach (var pair in node.Pairs)
            {
                KeyValuePair<KeyT, ValueT> deconvertedPair = DeconvertPair(pair, scheme);
                obj[deconvertedPair.Key] = deconvertedPair.Value;
            }
            return obj;
        }

        protected virtual KeyValuePair<INode, INode> ConvertPair(KeyValuePair<KeyT, ValueT> pair, IConverterScheme scheme)
        {
            INode key = ConvertNested(typeof(KeyT), pair.Key, scheme);
            INode value = ConvertNested(typeof(ValueT), pair.Value, scheme);
            return new(key, value);
        }

        protected virtual KeyValuePair<KeyT, ValueT> DeconvertPair(KeyValuePair<INode, INode> pair, IConverterScheme scheme)
        {
            KeyT key = DeconvertNested<KeyT>(pair.Key, scheme);
            ValueT value = DeconvertNested<ValueT>(pair.Value, scheme);
            return new(key, value);
        }
    }
}