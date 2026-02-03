#if NET6_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

// TODO: take into account fractional part of second.

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET time converter.
    /// </summary>
    public class TimeOnlyConverter : Core.Conversion.Converter<TimeOnly, TimeNode>
    {
        /* Protected method. */
        protected override TimeNode CreateNode(TimeOnly obj, CreateNodeContext context)
        {
            return new TimeNode(
                0, 0, 0,
                (byte)Math.Abs(obj.Hour), (byte)Math.Abs(obj.Minute), Math.Abs(obj.Second)
            );
        }

        protected override TimeOnly CreateObject(TimeNode node, CreateObjectContext context)
        {
            return new TimeOnly(
                (int)node.Value.hour, (int)node.Value.minute, (int)node.Value.second
            );
        }
    }
}
#endif