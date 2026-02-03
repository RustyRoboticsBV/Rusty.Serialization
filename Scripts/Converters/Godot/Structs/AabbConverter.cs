#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;
using System;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot AABB converter.
    /// </summary>
    public class AabbConverter : VectorConverter<Aabb, float>
    {
        /* Protected method. */
        protected override int GetLength() => 6;

        protected override float GetElementAt(ref Aabb vector, int index)
        {
            switch (index)
            {
                case 0: return vector.Position.X;
                case 1: return vector.Position.Y;
                case 2: return vector.Position.Z;
                case 3: return vector.Size.X;
                case 4: return vector.Size.Y;
                case 5: return vector.Size.Z;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Aabb vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.Position = new(element, vector.Position.Y, vector.Position.Z); break;
                case 1: vector.Position = new(vector.Position.X, element, vector.Position.Z); break;
                case 2: vector.Position = new(vector.Position.X, vector.Position.Y, element); break;
                case 3: vector.Size = new(element, vector.Size.Y, vector.Size.Z); break;
                case 4: vector.Size = new(vector.Size.X, element, vector.Size.Z); break;
                case 5: vector.Size = new(vector.Size.X, vector.Size.Y, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif