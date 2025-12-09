using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.Stack converter.
    /// </summary>
    public sealed class StackConverter<T> : GenericListConverter<Stack<T>, T>
    {
        /* Protected methods. */
        protected override Stack<T> CreateObject(ListNode node, IConverterScheme scheme, NodeTree tree) => new();

        protected override void AssignElements(Stack<T> collection, T[] elements)
        {
            for (int i = elements.Length; i >= 0; i--)
            {
                collection.Push(elements[i]);
            }
        }
    }
}