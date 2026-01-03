using System;
using System.Numerics;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET quaternion converter.
    /// </summary>
    public class QuaternionConverter : VectorConverter<Quaternion, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Quaternion vector, int index)
        {
            switch (index)
            {
                case 0:
                    return vector.X;
                case 1:
                    return vector.Y;
                case 2:
                    return vector.Z;
                case 3:
                    return vector.W;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Quaternion vector, int index, ref float element)
        {
            switch (index)
            {
                case 0:
                    vector.X = element; break;
                case 1:
                    vector.Y = element; break;
                case 2:
                    vector.Z = element; break;
                case 3:
                    vector.W = element; break;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }
    }
}