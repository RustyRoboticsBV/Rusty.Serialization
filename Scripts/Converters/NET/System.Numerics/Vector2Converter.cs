using System;
using System.Numerics;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET vector2 converter.
    /// </summary>
    public class Vector2Converter : VectorConverter<Vector2, float>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override float GetElementAt(ref Vector2 vector, int index)
        {
            switch (index)
            {
                case 0:
                    return vector.X;
                case 1:
                    return vector.Y;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Vector2 vector, int index, ref float element)
        {
            switch (index)
            {
                case 0:
                    vector.X = element; break;
                case 1:
                    vector.Y = element; break;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }
    }
}