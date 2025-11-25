using System.Collections.Generic;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A dictionary converter.
/// </summary>
public sealed class DictionaryConverter<KeyT, ValueT> : DictConverter<Dictionary<KeyT, ValueT>, KeyT, ValueT>
{
    
}