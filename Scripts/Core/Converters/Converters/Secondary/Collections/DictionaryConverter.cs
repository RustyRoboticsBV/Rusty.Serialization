using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

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
    }
}