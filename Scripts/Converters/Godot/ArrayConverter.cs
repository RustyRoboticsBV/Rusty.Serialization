#if GODOT
using Godot.Collections;
using System.Collections.Generic;
using Rusty.Serialization.Nodes;
using Godot;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A typed Godot array converter.
/// </summary>
public sealed class ArrayConverter<T> : EnumerableRefConverter<Array<T>, T, ListNode>
{
    /* Protected methods. */
    protected override ListNode CreateNode(INode[] elements) => new(elements);
    protected override Array<T> CreateObject(T[] elements) => new(elements);
    protected override IEnumerable<INode> GetElements(ListNode node) => node.Elements.ToArray();
}

/// <summary>
/// An untyped Godot array converter.
/// </summary>
public sealed class ArrayConverter : EnumerableRefConverter<Array, Variant, ListNode>
{
    /* Protected methods. */
    protected override ListNode CreateNode(INode[] elements) => new(elements);
    protected override Array CreateObject(Variant[] elements) => new(elements);
    protected override IEnumerable<INode> GetElements(ListNode node) => node.Elements.ToArray();
}
#endif