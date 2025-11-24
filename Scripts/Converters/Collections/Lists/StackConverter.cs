using System.Collections.Generic;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A stack converter.
/// </summary>
public sealed class StackConverter<T> : EnumerableRefConverter<Stack<T>, T, ListNode>
{
    /* Protected methods. */
    protected override ListNode CreateNode(INode[] elements) => new (elements);
    protected override Stack<T> CreateObject(T[] elements) => new(elements);
    protected override IEnumerable<INode> GetElements(ListNode node) => node.Elements.ToArray();
}