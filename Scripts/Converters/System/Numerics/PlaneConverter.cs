#if GODOT
using System;
using System.Numerics;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.Numerics.Plane converter.
/// </summary>
public sealed class PlaneConverter : VectorConverter<Plane, float>
{
    /* Protected properties. */
    protected override int Length => 4;

    /* Protected methods. */
    protected override float GetAt(ref Plane obj, int index)
    {
        switch (index)
        {
            case 0: return obj.Normal.X;
            case 1: return obj.Normal.Y;
            case 2: return obj.Normal.Z;
            case 3: return obj.D;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref Plane obj, int index, float value)
    {
        switch (index)
        {
            case 0: obj.Normal.X = value; break;
            case 1: obj.Normal.Y = value; break;
            case 2: obj.Normal.Z = value; break;
            case 3: obj.D = value; break;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }
}

#endif