#if GODOT
using System;
using System.Drawing;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.Drawing.Point converter.
/// </summary>
public sealed class PointConverter : VectorConverter<Point, int>
{
    /* Protected properties. */
    protected override int Length => 2;

    /* Protected methods. */
    protected override int GetAt(ref Point obj, int index)
    {
        switch (index)
        {
            case 0: return obj.X;
            case 1: return obj.Y;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref Point obj, int index, int value)
    {
        switch (index)
        {
            case 0: obj.X = value; break;
            case 1: obj.Y = value; break;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }
}
#endif