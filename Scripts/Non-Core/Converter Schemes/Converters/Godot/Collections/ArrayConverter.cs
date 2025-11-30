#if GODOT
using Godot;
using Godot.Collections;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A typed Godot array converter.
    /// </summary>
    public sealed class ArrayConverter<[MustBeVariant]T> : GenericListConverter<Array<T>, T>
    {
        /* Protected methods. */
        protected override Array<T> CreateObject(T[] elements) => new(elements);
    }

    /// <summary>
    /// An untyped Godot array converter.
    /// </summary>
    public sealed class ArrayConverter : GenericListConverter<Array, Variant>
    {
        /* Protected methods. */
        protected override Array CreateObject(Variant[] elements) => new(elements);
    }
}
#endif