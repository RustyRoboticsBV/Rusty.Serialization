using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An ISet converter.
    /// </summary>
    public abstract class SetConverter<SetT, ElementT> : CollectionConverter<SetT, ElementT, ListNode>
        where SetT : ISet<ElementT>
    {
        /* Protected methods. */
        protected override ListNode CreateNode(SetT obj, CreateNodeContext context)
        {
            return new(obj.Count);
        }

        protected override void AssignNode(ListNode node, SetT obj, AssignNodeContext context)
        {
            int index = 0;
            foreach (ElementT element in obj)
            {
                node.Elements[index] = context.CreateNode(typeof(SetT), element);
                index++;
            }
        }
    }
}