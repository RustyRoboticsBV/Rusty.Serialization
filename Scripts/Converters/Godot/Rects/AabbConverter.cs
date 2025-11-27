#if GODOT
using Godot;
using System;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Aabb converter.
    /// </summary>
    public sealed class AabbConverter : VectorConverter<Aabb, Vector3>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector3 GetAt(ref Aabb obj, int index)
        {
            switch (index)
            {
                case 0: return obj.Position;
                case 1: return obj.Size;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Aabb obj, int index, Vector3 value)
        {
            switch (index)
            {
                case 0: obj.Position = value; break;
                case 1: obj.Size = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif