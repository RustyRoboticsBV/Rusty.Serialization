using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A queue converter.
    /// </summary>
    public sealed class QueueConverter<T> : GenericListConverter<Queue<T>, T>
    {
        /* Protected methods. */
        protected override Queue<T> CreateObject(T[] elements) => new(elements);
    }
}