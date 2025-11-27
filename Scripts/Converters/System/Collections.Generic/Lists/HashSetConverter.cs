using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A hash set converter.
    /// </summary>
    public sealed class HashSetConverter<T> : GenericListConverter<HashSet<T>, T>
    {
        /* Protected methods. */
        protected override HashSet<T> CreateObject(T[] elements) => new(elements);
    }
}