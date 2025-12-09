using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.SortedSet converter.
    /// </summary>
    public sealed class SortedSetConverter<T> : GenericListConverter<SortedSet<T>, T>
    {
        /* Protected methods. */
        protected override SortedSet<T> CreateObject(ListNode node, IConverterScheme scheme, ParsingTable table) => new();

        protected override void AssignElements(SortedSet<T> collection, T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                collection.Add(elements[i]);
            }
        }
    }
}