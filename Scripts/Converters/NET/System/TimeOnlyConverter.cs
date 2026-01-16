#if NET6_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

// TODO: handle sign (?) and nanoseconds.
// TODO: update to new time literal.
/*
namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET time converter.
    /// </summary>
    public class TimeOnlyConverter : Core.Converters.Converter<TimeOnly, TimeNode>
    {
        /* Protected method. */
        protected override TimeNode CreateNode(TimeOnly obj, CreateNodeContext context)
        {
            return new(false,
                0, 0, 0,
                (ulong)Math.Abs(obj.Hour), (ulong)Math.Abs(obj.Minute), (ulong)Math.Abs(obj.Second),
                (ulong)Math.Abs(obj.Millisecond)
            );
        }

        protected override TimeOnly CreateObject(TimeNode node, CreateObjectContext context)
        {
            return new(
                (int)node.Hour, (int)node.Minute, (int)node.Second,
                (int)node.Millisecond
            );
        }
    }
}*/
#endif