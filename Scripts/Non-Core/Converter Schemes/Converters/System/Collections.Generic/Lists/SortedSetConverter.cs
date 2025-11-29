using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.SortedSet converter.
    /// </summary>
    public sealed class SortedSetConverter<T> : GenericListConverter<SortedSet<T>, T>
    {
        /* Protected methods. */
        protected override SortedSet<T> CreateObject(T[] elements) => new(elements);
    }
}