using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;
using System.Collections.Generic;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET linked list converter.
    /// </summary>
    public class LinkedListConverter<T> : CollectionConverter<LinkedList<T>, T, ListNode>
    {
        /* Protected methods. */
        protected override void AssignNode(ListNode node, LinkedList<T> obj, AssignNodeContext context)
        {
            int index = 0;
            foreach (T element in obj)
            {
                node.Elements[index] = context.CreateNode(typeof(LinkedList<T>), element);
                index++;
            }
        }
    }
}
