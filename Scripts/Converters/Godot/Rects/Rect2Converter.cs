#if GODOT
using System;
using Godot;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A Godot.Rect2 converter.
/// </summary>
public sealed class Rect2Converter : VectorConverter<Rect2, Vector2>
{
    /* Protected properties. */
    protected override int Length => 2;

    /* Protected methods. */
    protected override Vector2 GetAt(ref Rect2 obj, int index)
    {
        switch (index)
        {
            case 0: return obj.Position;
            case 1: return obj.Size;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }

    protected override void SetAt(ref Rect2 obj, int index, Vector2 value)
    {
        switch (index)
        {
            case 0: obj.Position = value; break;
            case 1: obj.Size = value; break;
            default: throw new ArgumentException($"Bad index {index}.");
        }
    }
}

#endif