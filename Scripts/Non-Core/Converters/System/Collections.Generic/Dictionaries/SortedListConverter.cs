using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A sorted list converter.
    /// </summary>
    public sealed class SortedListConverter<KeyT, ValueT> : GenericDictionaryConverter<SortedList<KeyT, ValueT>, KeyT, ValueT>
    { }
}