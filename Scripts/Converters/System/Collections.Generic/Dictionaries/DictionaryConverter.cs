using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A dictionary converter.
    /// </summary>
    public sealed class DictionaryConverter<KeyT, ValueT> : GenericDictionaryConverter<Dictionary<KeyT, ValueT>, KeyT, ValueT>
    { }
}