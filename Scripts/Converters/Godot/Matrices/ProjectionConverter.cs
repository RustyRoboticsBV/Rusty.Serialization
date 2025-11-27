#if GODOT
using Godot;
using System;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Projection converter.
    /// </summary>
    public sealed class ProjectionConverter : VectorConverter<Projection, Vector4>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override Vector4 GetAt(ref Projection obj, int index)
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

        protected override void SetAt(ref Projection obj, int index, Vector4 value)
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