using System.Collections.Generic;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET sorted dictionary converter.
    /// </summary>
    public class SortedDictionaryConverter<KeyT, ValueT> : DictionaryConverter<SortedDictionary<KeyT, ValueT>, KeyT, ValueT>
    {
    }
}
