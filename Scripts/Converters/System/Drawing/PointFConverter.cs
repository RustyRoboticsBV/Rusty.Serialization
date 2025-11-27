#if GODOT
using System;
using System.Drawing;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.Drawing.PointF converter.
/// </summary>
public sealed class PointFConverter : VectorConverter<PointF, float>
{
    /* Protected properties. */
    protected override int Length => 2;

    /* Protected methods. */
    protected override float GetAt(ref PointF obj, int index)
    {
        switch (index)
        {
            case 0: return obj.X;
            case 1: return obj.Y;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref PointF obj, int index, float value)
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