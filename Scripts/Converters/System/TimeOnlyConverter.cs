#if NET6_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.TimeOnly converter.
    /// </summary>
    public sealed class TimeOnlyConverter : ValueConverter<TimeOnly, TimeNode>
    {
        /* Protected methods. */
        protected override TimeNode Convert(TimeOnly obj, Context context)
            => new(new(0, 0, 0, obj.Hour, obj.Minute, obj.Second, obj.Millisecond));
        protected override TimeOnly Deconvert(TimeNode node, Context context)
            => new((int)node.Value.hour, (int)node.Value.minute, (int)node.Value.second, (int)node.Value.millisecond);
    }
}
#endif