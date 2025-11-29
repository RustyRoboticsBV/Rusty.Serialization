using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.DateTimeOffset converter.
    /// </summary>
    public sealed class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, ListNode>
    {
        /* Protected methods. */
        protected override ListNode ConvertValue(DateTimeOffset obj, IConverterScheme scheme)
        {
            DateTime dt = obj.DateTime;
            INode dtNode = new TimeNode(new(dt));

            TimeSpan tz = obj.Offset;
            INode tzNode = new TimeNode(new(tz));

            return new([dtNode, tzNode]);
        }

        protected override DateTimeOffset DeconvertValue(ListNode node, IConverterScheme scheme)
        {
            if (node.Elements.Length != 2)
                throw new ArgumentException("List node needs a length of 2.");

            DateTime dt = DeconvertNested<DateTime>(node.Elements[0], scheme);
            TimeSpan tz = DeconvertNested<TimeSpan>(node.Elements[1], scheme);
            return new(dt, tz);
        }
    }
}