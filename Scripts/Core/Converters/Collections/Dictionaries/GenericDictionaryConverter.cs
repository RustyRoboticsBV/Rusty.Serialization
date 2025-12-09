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
        protected sealed override DictNode CreateNode(DictionaryT obj, IConverterScheme scheme, SymbolTable table)
        {
            return new(obj.Count);
        }

        protected sealed override void AssignNode(ref DictNode node, DictionaryT obj, IConverterScheme scheme, SymbolTable table)
        {
            int index = 0;
            foreach (KeyValuePair<KeyT, ValueT> pair in obj)
            {
                KeyValuePair<INode, INode> nodePair = ConvertPair(pair, scheme, table);
                nodePair.Key.Parent = node;
                nodePair.Value.Parent = node;
                node.Pairs[index] = nodePair;
                index++;
            }
        }

        protected sealed override DictionaryT CreateObject(DictNode node, IConverterScheme scheme, NodeTree tree)
        {
            DictionaryT obj = new();
            foreach (var pair in node.Pairs)
            {
                KeyValuePair<KeyT, ValueT> deconvertedPair = DeconvertPair(pair, scheme, tree);
                obj[deconvertedPair.Key] = deconvertedPair.Value;
            }
            return obj;
        }

        protected virtual KeyValuePair<INode, INode> ConvertPair(KeyValuePair<KeyT, ValueT> pair, IConverterScheme scheme, SymbolTable table)
        {
            INode key = ConvertNested(typeof(KeyT), pair.Key, scheme, table);
            INode value = ConvertNested(typeof(ValueT), pair.Value, scheme, table);
            return new(key, value);
        }

        protected virtual KeyValuePair<KeyT, ValueT> DeconvertPair(KeyValuePair<INode, INode> pair, IConverterScheme scheme, NodeTree tree)
        {
            KeyT key = DeconvertNested<KeyT>(pair.Key, scheme, tree);
            ValueT value = DeconvertNested<ValueT>(pair.Value, scheme, tree);
            return new(key, value);
        }
    }
}