#if NET6_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.TimeOnly converter.
    /// </summary>
    public sealed class TimeOnlyConverter : ValueConverter<TimeOnly, TimeNode>
    {
        /* Protected methods. */
        protected override TimeNode ConvertValue(TimeOnly obj, IConverterScheme scheme, NodeTree tree)
            => new(new(0, 0, 0, obj.Hour, obj.Minute, obj.Second, obj.Millisecond));
        protected override TimeOnly DeconvertValue(TimeNode node, IConverterScheme scheme, NodeTree tree)
            => new((int)node.Value.hour, (int)node.Value.minute, (int)node.Value.second, (int)node.Value.millisecond);
    }
}
#endif