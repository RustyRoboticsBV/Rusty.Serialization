#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Basis converter.
    /// </summary>
    public sealed class BasisConverter : VectorConverter<Basis, Vector3>
    {
        /* Protected properties. */
        protected override int Length => 3;

        /* Protected methods. */
        protected override Vector3 GetAt(ref Basis obj, int index)
        {
            switch (index)
            {
                case 0: return obj.X;
                case 1: return obj.Y;
                case 2: return obj.Z;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Basis obj, int index, Vector3 value)
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
}
#endif