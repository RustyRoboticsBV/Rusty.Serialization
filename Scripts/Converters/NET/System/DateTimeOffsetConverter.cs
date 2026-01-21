using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date/time + offset converter.
    /// </summary>
    public class DateTimeOffsetConverter : Core.Conversion.Converter<DateTimeOffset, ListNode>
    {
        /* Protected method. */
        protected override ListNode CreateNode(DateTimeOffset obj, CreateNodeContext context)
        {
            INode dateTime = context.CreateNode(obj.DateTime);
            INode offset = context.CreateNode(obj.Offset);
            return new ListNode(dateTime, offset);
        }

        protected override DateTimeOffset CreateObject(ListNode node, CreateObjectContext context)
        {
            DateTime dateTime = context.CreateObject<DateTime>(node.Elements[0]);
            TimeSpan timeSpan = context.CreateObject<TimeSpan>(node.Elements[1]);
            return new DateTimeOffset(dateTime, timeSpan);
        }
    }
}