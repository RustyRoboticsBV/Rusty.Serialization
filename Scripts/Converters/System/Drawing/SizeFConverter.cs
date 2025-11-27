#if GODOT
using System;
using System.Drawing;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.Numerics.SizeF converter.
/// </summary>
public sealed class SizeFConverter : VectorConverter<SizeF, float>
{
    /* Protected properties. */
    protected override int Length => 2;

    /* Protected methods. */
    protected override float GetAt(ref SizeF obj, int index)
    {
        switch (index)
        {
            case 0: return obj.Width;
            case 1: return obj.Height;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref SizeF obj, int index, float value)
    {
        switch (index)
        {
            case 0: obj.Width = value; break;
            case 1: obj.Height = value; break;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }
}

#endif