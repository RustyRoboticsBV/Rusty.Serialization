using System.Collections.Generic;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET queue converter.
    /// </summary>
    public class QueueConverter<T> : EnumerableConverter<Queue<T>, T, ListNode>
    {
        protected override void AssignNode(ListNode node, Queue<T> obj, AssignNodeContext context)
        {
            int index = 0;
            foreach (INode item in node.Elements)
            {
                node.Elements[index] = context.CreateNode(item);
                index++;
            }
        }

        protected override Queue<T> AssignObject(Queue<T> obj, ListNode node, AssignObjectContext context)
        {
            for (int i = 0; i < node.Count; i++)
            {
                T element = context.CreateChildObject<T>(node.Elements[i]);
                obj.Enqueue(element);
            }
            return obj;
        }
    }
}
