using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.GenericHashSet converter.
    /// </summary>
    public sealed class HashSetConverter<T> : GenericListConverter<HashSet<T>, T>
    {
        /* Protected methods. */
        protected override HashSet<T> CreateObject(ListNode node, IConverterScheme scheme, ParsingTable table) => new();

        protected override void AssignElements(HashSet<T> collection, T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                collection.Add(elements[i]);
            }
        }
    }
}