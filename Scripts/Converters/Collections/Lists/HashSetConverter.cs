using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A hash set converter.
/// </summary>
public sealed class HashSetConverter<T> : EnumerableRefConverter<HashSet<T>, T, ListNode>
{
    /* Protected methods. */
    protected override ListNode CreateNode(INode[] elements) => new(elements);
    protected override HashSet<T> CreateObject(T[] elements) => new(elements);
    protected override IEnumerable<INode> GetElements(ListNode node) => node.Elements.ToArray();
}