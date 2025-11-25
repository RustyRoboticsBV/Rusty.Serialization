#if GODOT
using Godot;
using Godot.Collections;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A typed Godot.dictionary converter.
/// </summary>
public sealed class DictionaryConverter<KeyT, ValueT> : GenericDictionaryConverter<Dictionary<KeyT, ValueT>, KeyT, ValueT>
{

}

/// <summary>
/// An untyped Godot.dictionary converter.
/// </summary>
public sealed class DictionaryConverter : GenericDictionaryConverter<Dictionary, Variant, Variant>
{

}
#endif