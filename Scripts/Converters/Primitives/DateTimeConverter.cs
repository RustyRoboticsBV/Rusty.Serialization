using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A DateTime converter.
/// </summary>
public sealed class DateTimeConverter : ValueConverter<DateTime, TimestampNode>
{
    /* Protected methods. */
    protected override TimestampNode Convert(DateTime obj, Context context) => new(new(obj));
    protected override DateTime Deconvert(TimestampNode node, Context context)
        => new(node.Value.negative ? -(int)node.Value.year : (int)node.Value.year, (int)node.Value.month, (int)node.Value.day,
            (int)node.Value.hour, (int)node.Value.minute, (int)node.Value.second, (int)node.Value.millisecond);
}