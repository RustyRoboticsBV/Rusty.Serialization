#if GODOT
using Godot;
using Godot.Collections;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A typed Godot.dictionary converter.
    /// </summary>
    public sealed class DictionaryConverter<[MustBeVariant]KeyT, [MustBeVariant]ValueT> : GenericDictionaryConverter<Dictionary<KeyT, ValueT>, KeyT, ValueT>
    { }

    /// <summary>
    /// An untyped Godot.dictionary converter.
    /// </summary>
    public sealed class DictionaryConverter : GenericDictionaryConverter<Dictionary, Variant, Variant>
    { }
}
#endif