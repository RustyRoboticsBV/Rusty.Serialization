#if NET6_0_OR_GREATER
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;
using System.Linq;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.PriorityQueue converter.
    /// </summary>
    public sealed class PriorityQueueConverter<ElementT, PriorityT> : ReferenceConverter<PriorityQueue<ElementT, PriorityT>, DictNode>
    {
        /* Public methods. */
        protected override DictNode CreateNode(PriorityQueue<ElementT, PriorityT> obj, IConverterScheme scheme, SymbolTable table)
        {
            List<(ElementT, PriorityT)> elements = obj.UnorderedItems.ToList();
            var pairs = new KeyValuePair<INode, INode>[elements.Count];
            for (int i = 0; i < pairs.Length; i++)
            {
                INode elementNode = ConvertNested(typeof(ElementT), elements[i].Item1, scheme, table);
                INode priorityNode = ConvertNested(typeof(PriorityT), elements[i].Item2, scheme, table);
                pairs[i] = new(elementNode, priorityNode);
            }
            return new(pairs);
        }

        protected override PriorityQueue<ElementT, PriorityT> DeconvertRef(DictNode node, IConverterScheme scheme, NodeTree tree)
        {
            PriorityQueue<ElementT, PriorityT> pqueue = new();
            for (int i = 0; i < node.Pairs.Length; i++)
            {
                ElementT element = DeconvertNested<ElementT>(node.Pairs[i].Key, scheme, tree);
                PriorityT priority = DeconvertNested<PriorityT>(node.Pairs[i].Value, scheme, tree);
                pqueue.Enqueue(element, priority);
            }
            return pqueue;
        }
    }
}
#endif