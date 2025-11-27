#if GODOT
using System;
using System.Numerics;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.Numerics.Vector3 converter.
/// </summary>
public sealed class Vector3Converter : VectorConverter<Vector3, float>
{
    /* Protected properties. */
    protected override int Length => 3;

    /* Protected methods. */
    protected override float GetAt(ref Vector3 obj, int index)
    {
        switch (index)
        {
            case 0: return obj.X;
            case 1: return obj.Y;
            case 2: return obj.Z;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref Vector3 obj, int index, float value)
    {
        switch (index)
        {
            case 0: obj.X = value; break;
            case 1: obj.Y = value; break;
            case 2: obj.Z = value; break;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }
}

#endif