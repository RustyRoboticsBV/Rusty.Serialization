#if NET6_0_OR_GREATER
using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET priority queue converter.
    /// </summary>
    public sealed class PriorityQueueConverter<ElementT, PriorityT> : Converter<PriorityQueue<ElementT, PriorityT>, DictNode>, ICompositeConverter
    {
        /* Public methods. */
        void ICompositeConverter.AssignNode(INode node, object obj, AssignNodeContext context)
            => AssignNode((DictNode)node, (PriorityQueue<ElementT, PriorityT>)obj, context);
        object ICompositeConverter.AssignObject(object obj, INode node, AssignObjectContext context)
            => AssignObject((DictNode)node, (PriorityQueue<ElementT, PriorityT>)obj, context);

        /* Protected methods. */
        protected override void CollectTypes(DictNode node, CollectTypesContext context)
        {
            for (int i = 0; i < node.Count; i++)
            {
                CollectTypes(node.GetKeyAt(i), context);
                CollectTypes(node.GetValueAt(i), context);
            }
        }

        protected override DictNode CreateNode(PriorityQueue<ElementT, PriorityT> obj, CreateNodeContext context)
        {
            return new DictNode(obj.Count);
        }

        protected override PriorityQueue<ElementT, PriorityT> CreateObject(DictNode node, CreateObjectContext context)
        {
            return new PriorityQueue<ElementT, PriorityT>(node.Count);
        }

        /* Private methods. */
        private void AssignNode(DictNode node, PriorityQueue<ElementT, PriorityT> obj, AssignNodeContext context)
        {
            int index = 0;
            foreach (var pair in obj.UnorderedItems)
            {
                INode element = context.CreateNode(typeof(ElementT), pair.Element);
                INode priority = context.CreateNode(typeof(PriorityT), pair.Priority);
                node.Pairs[index] = new KeyValuePair<INode, INode>(element, priority);
                index++;
            }
        }

        private PriorityQueue<ElementT, PriorityT> AssignObject(DictNode node, PriorityQueue<ElementT, PriorityT> obj, AssignObjectContext context)
        {
            for (int i = 0; i < node.Count; i++)
            {
                ElementT element = context.CreateChildObject<ElementT>(node.GetKeyAt(i));
                PriorityT priority = context.CreateChildObject<PriorityT>(node.GetValueAt(i));
                obj.Enqueue(element, priority);
            }
            return obj;
        }
    }
}
#endif