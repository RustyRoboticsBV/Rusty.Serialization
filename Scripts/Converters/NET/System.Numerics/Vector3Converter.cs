using System;
using System.Numerics;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET vector3 converter.
    /// </summary>
    public class Vector3Converter : VectorConverter<Vector3, float>
    {
        /* Protected method. */
        protected override int GetLength() => 3;

        protected override float GetElementAt(ref Vector3 vector, int index)
        {
            switch (index)
            {
                case 0:
                    return vector.X;
                case 1:
                    return vector.Y;
                case 2:
                    return vector.Z;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Vector3 vector, int index, ref float element)
        {
            switch (index)
            {
                case 0:
                    vector.X = element; break;
                case 1:
                    vector.Y = element; break;
                case 2:
                    vector.Z = element; break;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }
    }
}