#if GODOT
using System;
using System.Drawing;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.Numerics.Size converter.
/// </summary>
public sealed class SizeConverter : VectorConverter<Size, int>
{
    /* Protected properties. */
    protected override int Length => 2;

    /* Protected methods. */
    protected override int GetAt(ref Size obj, int index)
    {
        switch (index)
        {
            case 0: return obj.Width;
            case 1: return obj.Height;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref Size obj, int index, int value)
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