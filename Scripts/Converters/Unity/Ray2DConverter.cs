using System;
using UnityEngine;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity 2D ray converter.
    /// </summary>
    public class Ray2DConverter : MatrixConverter<Ray2D, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;
        protected override int GetWidth() => 2;

        protected override float GetElementAt(ref Ray2D matrix, int x, int y)
        {
            switch (y)
            {
                case 0:
                    return matrix.origin[x];
                case 1:
                    return matrix.direction[x];
                default:
                    throw new ArgumentOutOfRangeException(nameof(x) + " " + nameof(y));
            }
        }

        protected override void SetElementAt(ref Ray2D matrix, int x, int y, ref float element)
        {
            switch (y)
            {
                case 0:
                    switch (x)
                    {
                        case 0:
                            matrix.origin = new Vector2(element, matrix.origin.y); break;
                        case 1:
                            matrix.origin = new Vector2(matrix.origin.x, element); break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(x));
                    }
                    break;
                case 1:
                    switch (x)
                    {
                        case 0:
                            matrix.direction = new Vector2(element, matrix.direction.y); break;
                        case 1:
                            matrix.direction = new Vector2(matrix.direction.x, element); break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(x));
                    }
                    break;
            }
        }
    }
}