using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.Generic.SortedDictionary converter.
    /// </summary>
    public sealed class SortedDictionaryConverter<KeyT, ValueT> : GenericDictionaryConverter<SortedDictionary<KeyT, ValueT>, KeyT, ValueT>
    { }
}