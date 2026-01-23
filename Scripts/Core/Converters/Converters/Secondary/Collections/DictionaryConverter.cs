using Rusty.Serialization.Core.Nodes;
using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An IList converter.
    /// </summary>
    public abstract class DictionaryConverter<DictT, KeyT, ValueT> : CollectionConverter<DictT, KeyValuePair<KeyT, ValueT>, DictNode>
        where DictT : IDictionary<KeyT, ValueT>
    {
        /* Protected methods. */
        protected override DictNode CreateNode(DictT obj, CreateNodeContext context)
        {
            return new(obj.Count);
        }

        protected override void AssignNode(DictNode node, DictT obj, AssignNodeContext context)
        {
            Type keyType = typeof(KeyT);
            Type valueType = typeof(ValueT);
            int index = 0;
            foreach (KeyValuePair<KeyT, ValueT> pair in obj)
            {
                INode key = context.CreateNode(keyType, pair.Key);
                INode value = context.CreateNode(valueType, pair.Value);
                node.Pairs[index] = new(key, value);
                index++;
            }
        }

        protected override void CollectTypes(DictNode node, CollectTypesContext context)
        {
            Type keyType = typeof(KeyT);
            Type valueType = typeof(ValueT);
            for (int i = 0; i < node.Count; i++)
            {
                context.CollectTypes(node.Pairs[i].Key, keyType);
                context.CollectTypes(node.Pairs[i].Value, valueType);
            }
        }

        protected override DictT CreateObject(DictNode node, CreateObjectContext context)
            => (DictT)Activator.CreateInstance(typeof(DictT));

        protected override DictT AssignObject(DictT obj, DictNode node, AssignObjectContext context)
        {
            Type keyType = typeof(KeyT);
            Type valueType = typeof(ValueT);
            for (int i = 0; i < node.Count; i++)
            {
                KeyT key = (KeyT)context.CreateChildObject(keyType, node.GetKeyAt(i));
                ValueT value = (ValueT)context.CreateChildObject(valueType, node.GetValueAt(i));
                obj.Add(key, value);
            }
            return obj;
        }
    }
}