using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET time span converter.
    /// </summary>
    public class TimeSpanConverter : Core.Conversion.Converter<TimeSpan, IntNode>
    {
        /* Protected method. */
        protected override IntNode CreateNode(TimeSpan obj, CreateNodeContext context) => new IntNode(obj.Ticks);
        protected override TimeSpan CreateObject(IntNode node, CreateObjectContext context) => new TimeSpan((long)node.Value);
    }
}