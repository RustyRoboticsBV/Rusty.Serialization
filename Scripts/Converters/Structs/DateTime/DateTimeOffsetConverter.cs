using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A DateTimeOffset converter.
/// </summary>
public sealed class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DatetimeNode>
{
    /* Protected methods. */
    protected override DatetimeNode Convert(DateTimeOffset obj, Context context) => new(obj);
    protected override DateTimeOffset Deconvert(DatetimeNode node, Context context) => node.Value;
}