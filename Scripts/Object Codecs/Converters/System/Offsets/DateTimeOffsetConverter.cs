using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A date/time/offset converter.
    /// </summary>
    public sealed class DateTimeOffsetConverter : Converter
    {
        /* Private properties. */
        private DateTimeConverter DateTimeConverter { get; } = new DateTimeConverter();

        /* Piblic methods. */
        public override INode CreateNode(object obj, CreateNodeContext context)
        {
            // Extract components.
            DateTimeOffset dateTimeOffset = (DateTimeOffset)obj;
            DateTime dateTime = dateTimeOffset.DateTime;
            TimeSpan offset = dateTimeOffset.Offset;

            // Convert date/time.
            TimestampNode timestampNode = (TimestampNode)DateTimeConverter.CreateNode(dateTime, context);

            // Convert offset.
            OffsetValue offsetValue = new OffsetValue(offset.Ticks < 0, offset.Hours, offset.Minutes);
            OffsetNode offsetNode = new OffsetNode(offsetValue, timestampNode);
            return offsetNode;
        }

        /* Protected methods. */
        protected override object CreateObject(OffsetNode node, CreateObjectContext context)
        {
            return new DateTimeOffset(
                (DateTime)CreateObject(node.Child, context),
                new TimeSpan((int)node.Value.hours, (int)node.Value.minutes, 0)
            );
        }

        protected override object CreateObject(TimestampNode node, CreateObjectContext context)
        {
            return DateTimeConverter.CreateObject(node, context);
        }
    }
}