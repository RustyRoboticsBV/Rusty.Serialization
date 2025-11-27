#if GODOT
using Godot;
using System;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Transform3D converter.
    /// </summary>
    public sealed class Transform3DConverter : VectorConverter<Transform3D, Vector3>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override Vector3 GetAt(ref Transform3D obj, int index)
        {
            switch (index)
            {
                case 0: return obj.Basis.X;
                case 1: return obj.Basis.Y;
                case 2: return obj.Basis.Z;
                case 3: return obj.Origin;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Transform3D obj, int index, Vector3 value)
        {
            switch (index)
            {
                case 0: obj.Basis.X = value; break;
                case 1: obj.Basis.Y = value; break;
                case 2: obj.Basis.Z = value; break;
                case 3: obj.Origin = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif