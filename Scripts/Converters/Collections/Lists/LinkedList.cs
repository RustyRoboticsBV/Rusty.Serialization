using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A linked list converter.
/// </summary>
public sealed class LinkedListConverter<T> : EnumerableRefConverter<LinkedList<T>, T, ListNode>
{
    /* Protected methods. */
    protected override ListNode CreateNode(INode[] elements) => new(elements);
    protected override LinkedList<T> CreateObject(T[] elements) => new(elements);
    protected override IEnumerable<INode> GetElements(ListNode node) => node.Elements.ToArray();
}