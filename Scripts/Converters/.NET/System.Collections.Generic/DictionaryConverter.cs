using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET dictionary converter.
    /// </summary>
    public class DictionaryConverter<KeyT, ValueT> : DictionaryConverter<Dictionary<KeyT, ValueT>, KeyT, ValueT>
    {
    }
}
