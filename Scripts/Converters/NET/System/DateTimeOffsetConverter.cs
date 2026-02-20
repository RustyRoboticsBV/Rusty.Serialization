using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date/time + offset converter.
    /// </summary>
    public class DateTimeOffsetConverter : Core.Conversion.Converter<DateTimeOffset, OffsetNode>
    {
        /* Protected method. */
        protected override OffsetNode CreateNode(DateTimeOffset obj, CreateNodeContext context)
        {
            TimestampNode timestamp = (TimestampNode)context.CreateNode(obj.DateTime);
            OffsetValue offset = new OffsetValue(obj.Offset.Ticks < 0, obj.Offset.Hours, obj.Offset.Minutes);
            return new OffsetNode(offset, timestamp);
        }

        protected override DateTimeOffset CreateObject(OffsetNode node, CreateObjectContext context)
        {
            DateTime dateTime = context.CreateObject<DateTime>(node.Time);
            TimeSpan timeSpan = new TimeSpan((int)node.Offset.hours, (int)node.Offset.minutes, 0);
            return new DateTimeOffset(dateTime, timeSpan);
        }
    }
}