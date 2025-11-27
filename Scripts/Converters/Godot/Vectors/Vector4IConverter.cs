#if GODOT
using Godot;
using System;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Vector4I converter.
    /// </summary>
    public sealed class Vector4IConverter : VectorConverter<Vector4I, int>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override int GetAt(ref Vector4I obj, int index)
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

        protected override void SetAt(ref Vector4I obj, int index, int value)
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
}
#endif