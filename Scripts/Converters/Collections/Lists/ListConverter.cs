using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A list converter.
/// </summary>
public sealed class ListConverter<T> : EnumerableRefConverter<List<T>, T, ListNode>
{
    /* Protected methods. */
    protected override ListNode CreateNode(INode[] elements) => new(elements);
    protected override List<T> CreateObject(T[] elements) => new(elements);
    protected override IEnumerable<INode> GetElements(ListNode node) => node.Elements.ToArray();
}