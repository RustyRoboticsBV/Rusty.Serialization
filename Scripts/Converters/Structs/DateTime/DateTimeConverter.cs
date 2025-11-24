using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A DateTime converter.
/// </summary>
public sealed class DateTimeConverter : ValueConverter<DateTime, DatetimeNode>
{
    /* Protected methods. */
    protected override DatetimeNode Convert(DateTime obj, Context context) => new(obj);
    protected override DateTime Deconvert(DatetimeNode node, Context context) => node.Value;
}