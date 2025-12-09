using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.LinkedList converter.
    /// </summary>
    public sealed class LinkedListConverter<T> : GenericListConverter<LinkedList<T>, T>
    {
        /* Protected methods. */
        protected override LinkedList<T> CreateObject(ListNode node, IConverterScheme scheme, NodeTree tree) => new();

        protected override void AssignElements(LinkedList<T> collection, T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                collection.AddLast(elements[i]);
            }
        }
    }
}