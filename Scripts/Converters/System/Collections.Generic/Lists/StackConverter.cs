using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A stack converter.
    /// </summary>
    public sealed class StackConverter<T> : GenericListConverter<Stack<T>, T>
    {
        /* Protected methods. */
        protected override Stack<T> CreateObject(T[] elements) => new(elements);
    }
}