using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date/time + offset converter.
    /// </summary>
    public class DateTimeOffsetConverter : Core.Conversion.Converter<DateTimeOffset, INode>
    {
        /* Constructors. */
        public DateTimeOffsetConverter()
        {
            AllowedNodeTypes = new Type[2] { typeof(TimestampNode), typeof(OffsetNode) };
        }

        /* Protected method. */
        protected override INode CreateNode(DateTimeOffset obj, CreateNodeContext context)
        {
            TimestampNode timestamp = (TimestampNode)context.CreateNode(obj.DateTime);
            OffsetValue offset = new OffsetValue(obj.Offset.Ticks < 0, obj.Offset.Hours, obj.Offset.Minutes);
            return new OffsetNode(offset, timestamp);
        }

        protected override DateTimeOffset CreateObject(INode node, CreateObjectContext context)
        {
            if (node is OffsetNode offset)
            {
                DateTime dateTime = context.CreateObject<DateTime>(offset.Child);
                TimeSpan timeSpan = new TimeSpan((int)offset.Value.hours, (int)offset.Value.minutes, 0);
                return new DateTimeOffset(dateTime, timeSpan);
            }
            else if (node is TimestampNode timestamp)
            {
                DateTime dateTime = context.CreateObject<DateTime>(timestamp);
                return new DateTimeOffset(dateTime);
            }

            throw new ArgumentException();
        }
    }
}