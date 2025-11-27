#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Plane converter.
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
                case 0: return obj.X;
                case 1: return obj.Y;
                case 2: return obj.Z;
                case 3: return obj.D;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Plane obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.X = value; break;
                case 1: obj.Y = value; break;
                case 2: obj.Z = value; break;
                case 3: obj.D = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif