using System.Collections.Generic;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET sorted list converter.
    /// </summary>
    public class SortedListConverter<KeyT, ValueT> : DictionaryConverter<SortedList<KeyT, ValueT>, KeyT, ValueT>
    {
    }
}
