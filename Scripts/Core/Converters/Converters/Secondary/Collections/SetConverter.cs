using Rusty.Serialization.Core.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Conversion
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

        protected override void CollectTypes(ListNode node, CollectTypesContext context)
        {
            Type elementType = typeof(SetT).GetGenericArguments()[0];

            for (int i = 0; i < node.Count; i++)
            {
                context.CollectTypesAndReferences(node.Elements[i], elementType);
            }
        }
    }
}