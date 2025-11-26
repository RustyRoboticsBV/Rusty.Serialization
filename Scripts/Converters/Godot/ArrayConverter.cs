#if GODOT
using Godot.Collections;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;
using Godot;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A typed Godot array converter.
/// </summary>
public sealed class ArrayConverter<T> : GenericListConverter<Array<T>, T>
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
#endif