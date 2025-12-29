#if NETCOREAPP3_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET range converter.
    /// </summary>
    public class RangeConverter : Core.Converters.Converter<Range, ListNode>
    {
        /* Protected method. */
        protected override ListNode CreateNode(Range obj, CreateNodeContext context)
        {
            ListNode node = new ListNode(2);
            node.Elements[0] = new IntNode(obj.Start.Value);
            node.Elements[1] = new IntNode(obj.End.Value);
            return node;
        }

        protected override Range CreateObject(ListNode node, CreateObjectContext context)
        {
            Index start = (Index)context.CreateObject(typeof(Index), node.Elements[0]);
            Index end = (Index)context.CreateObject(typeof(Index), node.Elements[1]);
            return new(start, end);
        }
    }
}
#endif