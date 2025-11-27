#if GODOT
using System;
using Godot;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A Godot.Vector4 converter.
/// </summary>
public sealed class Vector4Converter : VectorConverter<Vector4, float>
{
    /* Protected properties. */
    protected override int Length => 4;

    /* Protected methods. */
    protected override float GetAt(ref Vector4 obj, int index)
    {
        switch (index)
        {
            case 0: return obj.X;
            case 1: return obj.Y;
            case 2: return obj.Z;
            case 3: return obj.W;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref Vector4 obj, int index, float value)
    {
        switch (index)
        {
            case 0: obj.X = value; break;
            case 1: obj.Y = value; break;
            case 2: obj.Z = value; break;
            case 3: obj.W = value; break;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }
}

#endif