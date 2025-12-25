using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET date/time converter.
    /// </summary>
    public class DateTimeConverter : Core.Converters.Converter<DateTime, TimeNode>
    {
        /* Protected method. */
        protected override TimeNode CreateNode(DateTime obj, CreateNodeContext context)
        {
            return new(false,
                (ulong)Math.Abs(obj.Year), (ulong)Math.Abs(obj.Month), (ulong)Math.Abs(obj.Day),
                (ulong)Math.Abs(obj.Hour), (ulong)Math.Abs(obj.Minute), (ulong)Math.Abs(obj.Second), (ulong)Math.Abs(obj.Millisecond));
        }

        protected override DateTime CreateObject(TimeNode node, CreateObjectContext context)
        {
            return new((int)node.Year, (int)node.Month, (int)node.Day,
                (int)node.Hour, (int)node.Minute, (int)node.Second, (int)node.Millisecond);
        }
    }
}
