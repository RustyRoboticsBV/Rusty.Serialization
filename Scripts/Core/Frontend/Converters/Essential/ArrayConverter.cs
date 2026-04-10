using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An array converter.
    /// </summary>
    public sealed class ArrayConverter<T> : Converter
    {
        protected override void CollectChildNodeTypes(ListNode node, CollectTypesContext context)
        {
            Type elementType = typeof(T);
            for (int i = 0; i < node.Count; i++)
            {
                context.Collect(node.GetValueAt(i), elementType);
            }
        }

        public override INode CreateNode(object obj, CreateNodeContext context)
        {
            return new ListNode();
        }
    }
}