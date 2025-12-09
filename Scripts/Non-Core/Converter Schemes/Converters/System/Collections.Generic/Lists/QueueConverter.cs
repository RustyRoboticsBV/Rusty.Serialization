using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.Queue converter.
    /// </summary>
    public sealed class QueueConverter<T> : GenericListConverter<Queue<T>, T>
    {
        /* Protected methods. */
        protected override Queue<T> CreateObject(ListNode node, IConverterScheme scheme, ParsingTable table) => new();

        protected override void AssignElements(Queue<T> collection, T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                collection.Enqueue(elements[i]);
            }
        }
    }
}