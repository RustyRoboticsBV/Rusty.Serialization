using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
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