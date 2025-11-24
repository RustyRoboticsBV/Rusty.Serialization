using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A queue converter.
/// </summary>
public sealed class QueueConverter<T> : EnumerableRefConverter<Queue<T>, T, ListNode>
{
    /* Protected methods. */
    protected override ListNode CreateNode(INode[] elements) => new(elements);
    protected override Queue<T> CreateObject(T[] elements) => new(elements);
    protected override IEnumerable<INode> GetElements(ListNode node) => node.Elements.ToArray();
}