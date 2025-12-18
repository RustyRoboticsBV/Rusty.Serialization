using Rusty.Serialization.Core.Nodes;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Rusty.Serialization.Core.Converters
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
            int index = 0;
            foreach (KeyValuePair<KeyT, ValueT> pair in obj)
            {
                INode key = context.CreateNode(typeof(DictT), pair.Key);
                INode value = context.CreateNode(typeof(DictT), pair.Value);
                node.Pairs[index] = new(key, value);
                index++;
            }
        }

        protected override DictT CreateObject(DictNode node, CreateObjectContext context)
        {
            List<KeyValuePair<KeyT, ValueT>> pairs = new();
            for (int i = 0; i < node.Count; i++)
            {
                KeyT key = context.CreateObject<KeyT>(node.GetKeyAt(i));
                ValueT value = context.CreateObject<ValueT>(node.GetValueAt(i));
                if (key == null)
                    continue;
                pairs.Add(new(key, value));
            }
            return CreateObjectFromElements(pairs);
        }

        protected override DictT FixReferences(DictT obj, DictNode node, FixReferencesContext context)
        {
            List<KeyValuePair<KeyT, ValueT>> pairs = new();
            int index = 0;
            foreach (var pair in obj)
            {
                KeyT key = (KeyT)context.FixReferences(pair.Key, node.GetKeyAt(index));
                ValueT value = (ValueT)context.FixReferences(pair.Value, node.GetValueAt(index));
                index++;
            }
            return CreateObjectFromElements(pairs);
        }
    }
}