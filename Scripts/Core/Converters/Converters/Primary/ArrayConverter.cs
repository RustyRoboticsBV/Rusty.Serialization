using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An array converter.
    /// </summary>
    public sealed class ArrayConverter<T> : ListConverter<T[], T>
    {
        /* Protected methods. */
        protected override T[] CreateObjectFromElements(ICollection<T> elements)
        {
            if (elements is T[] array)
                return array;
            else
                return elements.ToArray();
        }
    }
}