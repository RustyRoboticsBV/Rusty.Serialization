using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.KeyValuePair converter.
    /// </summary>
    public sealed class KeyValuePairConverter<KeyT, ValueT> : ValueConverter<KeyValuePair<KeyT, ValueT>, DictNode>
    {
        /* Protected methods. */
        protected sealed override DictNode ConvertValue(KeyValuePair<KeyT, ValueT> obj, IConverterScheme nested, SymbolTable table)
        {
            INode key = ConvertNested(typeof(KeyT), obj.Key, nested, table);
            INode value = ConvertNested(typeof(ValueT), obj.Value, nested, table);
            return new(new KeyValuePair<INode, INode>[1] { new(key, value) });
        }

        protected sealed override KeyValuePair<KeyT, ValueT> DeconvertValue(DictNode node, IConverterScheme nested, ParsingTable table)
        {
            if (node.Pairs.Length != 1)
                throw new ArgumentException("Cannot deserialize dict node with length that isn't 1.");
            KeyT key = DeconvertNested<KeyT>(node.Pairs[0].Key, nested, table);
            ValueT value = DeconvertNested<ValueT>(node.Pairs[0].Value, nested, table);
            return new(key, value);
        }
    }
}