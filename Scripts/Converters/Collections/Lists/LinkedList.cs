using System.Collections.Generic;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A linked list converter.
    /// </summary>
    public sealed class LinkedListConverter<T> : GenericListConverter<LinkedList<T>, T>
    {
        /* Protected methods. */
        protected override LinkedList<T> CreateObject(T[] elements) => new(elements);
    }
}