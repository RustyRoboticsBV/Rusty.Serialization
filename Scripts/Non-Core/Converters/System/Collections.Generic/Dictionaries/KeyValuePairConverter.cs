using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A key-value pair converter.
    /// </summary>
    public sealed class KeyValuePairConverter<KeyT, ValueT> : ValueConverter<KeyValuePair<KeyT, ValueT>, DictNode>
    {
        /* Protected methods. */
        protected sealed override DictNode ConvertValue(KeyValuePair<KeyT, ValueT> obj, IConverterScheme nested)
        {
            INode key = ConvertNested(typeof(KeyT), obj.Key, nested);
            INode value = ConvertNested(typeof(ValueT), obj.Value, nested);
            return new([new(key, value)]);
        }

        protected sealed override KeyValuePair<KeyT, ValueT> DeconvertValue(DictNode node, IConverterScheme nested)
        {
            if (node.Pairs.Length != 1)
                throw new ArgumentException("Cannot deserialize dict node with length that isn't 1.");
            KeyT key = DeconvertNested<KeyT>(node.Pairs[0].Key, nested);
            ValueT value = DeconvertNested<ValueT>(node.Pairs[0].Value, nested);
            return new(key, value);
        }
    }
}