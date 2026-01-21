using System.Collections.Generic;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET stack converter.
    /// </summary>
    public class StackConverter<T> : EnumerableConverter<Stack<T>, T, ListNode>
    {
        protected override void AssignNode(ListNode node, Stack<T> obj, AssignNodeContext context)
        {
            int index = 0;
            foreach (INode item in node.Elements)
            {
                node.Elements[index] = context.CreateNode(item);
                index++;
            }
        }

        protected override Stack<T> AssignObject(Stack<T> obj, ListNode node, AssignObjectContext context)
        {
            for (int i = node.Count - 1; i >= 0; i--)
            {
                T element = context.CreateChildObject<T>(node.Elements[i]);
                obj.Push(element);
            }
            return obj;
        }
    }
}
