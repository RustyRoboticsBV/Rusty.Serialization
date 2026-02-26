using System.Collections.Generic;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET key-value pair converter.
    /// </summary>
    public class KeyValuePairConverter<KeyT, ValueT> : Converter<KeyValuePair<KeyT, ValueT>, DictNode>
    {
        /* Protected methods. */
        protected override void CollectTypes(DictNode node, CollectTypesContext context)
        {
            context.CollectTypesAndReferences(node.GetKeyAt(0), typeof(KeyT));
            context.CollectTypesAndReferences(node.GetValueAt(0), typeof(ValueT));
        }

        protected override DictNode CreateNode(KeyValuePair<KeyT, ValueT> obj, CreateNodeContext context)
        {
            INode key = context.CreateNode(obj.Key);
            INode value = context.CreateNode(obj.Value);

            DictNode node = new DictNode(1);
            node.Pairs[0] = new KeyValuePair<INode, INode>(key, value);
            return node;
        }

        protected override KeyValuePair<KeyT, ValueT> CreateObject(DictNode node, CreateObjectContext context)
        {
            return new KeyValuePair<KeyT, ValueT>(
                context.CreateObject<KeyT>(node.GetKeyAt(0)),
                context.CreateObject<ValueT>(node.GetValueAt(0))
            );
        }
    }
}
