using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An IList converter.
    /// </summary>
    public abstract class ListConverter<ListT, ElementT> : CollectionConverter<ListT, ElementT, ListNode>
        where ListT : IList<ElementT>
    {
        /* Protected methods. */
        protected override ListNode CreateNode(ListT obj, CreateNodeContext context)
        {
            return new(obj.Count);
        }

        protected override void AssignNode(ListNode node, ListT obj, AssignNodeContext context)
        {
            int index = 0;
            foreach (ElementT element in obj)
            {
                node.Elements[index] = context.CreateNode(typeof(ListT), element);
                index++;
            }
        }
    }
}