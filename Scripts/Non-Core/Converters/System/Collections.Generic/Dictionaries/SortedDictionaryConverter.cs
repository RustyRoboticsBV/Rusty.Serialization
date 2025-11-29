using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A sorted dictionary converter.
    /// </summary>
    public sealed class SortedDictionaryConverter<KeyT, ValueT> : GenericDictionaryConverter<SortedDictionary<KeyT, ValueT>, KeyT, ValueT>
    { }
}