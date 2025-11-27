using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.TimeSpan converter.
/// </summary>
public sealed class TimeSpanConverter : ValueConverter<TimeSpan, TimeNode>
{
    /* Protected methods. */
    protected override TimeNode Convert(TimeSpan obj, Context context) => new(new(obj));
    protected override TimeSpan Deconvert(TimeNode node, Context context)
        => new(node.Value.negative ? -(int)node.Value.day : (int)node.Value.day,
            (int)node.Value.hour, (int)node.Value.minute, (int)node.Value.second, (int)node.Value.millisecond);
}