using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A list converter.
    /// </summary>
    public sealed class ListConverter<T> : GenericListConverter<List<T>, T>
    {
        /* Protected methods. */
        protected override List<T> CreateObject(T[] elements) => new(elements);
    }
}