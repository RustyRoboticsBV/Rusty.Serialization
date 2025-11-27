#if GODOT
using System;
using Godot;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A Godot.Vector3I converter.
/// </summary>
public sealed class Vector3IConverter : VectorConverter<Vector3I, int>
{
    /* Protected properties. */
    protected override int Length => 3;

    /* Protected methods. */
    protected override int GetAt(ref Vector3I obj, int index)
    {
        switch (index)
        {
            case 0: return obj.X;
            case 1: return obj.Y;
            case 2: return obj.Z;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref Vector3I obj, int index, int value)
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