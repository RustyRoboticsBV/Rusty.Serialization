using System;
using System.Numerics;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET plane converter.
    /// </summary>
    public class PlaneConverter : VectorConverter<Plane, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Plane vector, int index)
        {
            switch (index)
            {
                case 0: return vector.Normal.X;
                case 1: return vector.Normal.Y;
                case 2: return vector.Normal.Z;
                case 3: return vector.D;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Plane vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.Normal.X = element; break;
                case 1: vector.Normal.Y = element; break;
                case 2: vector.Normal.Z = element; break;
                case 3: vector.D = element; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}