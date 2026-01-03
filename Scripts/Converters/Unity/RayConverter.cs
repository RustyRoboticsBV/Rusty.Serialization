using System;
using UnityEngine;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity ray converter.
    /// </summary>
    public class RayConverter : MatrixConverter<Ray, float>
    {
        /* Protected method. */
        protected override int GetLength() => 6;
        protected override int GetWidth() => 3;

        protected override float GetElementAt(ref Ray matrix, int x, int y)
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

        protected override void SetElementAt(ref Ray matrix, int x, int y, ref float element)
        {
            switch (y)
            {
                case 0:
                    switch (x)
                    {
                        case 0:
                            matrix.origin = new Vector3(element, matrix.origin.y, matrix.origin.z); break;
                        case 1:
                            matrix.origin = new Vector3(matrix.origin.x, element, matrix.origin.z); break;
                        case 2:
                            matrix.origin = new Vector3(matrix.origin.x, matrix.origin.y, element); break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(x));
                    }
                    break;
                case 1:
                    switch (x)
                    {
                        case 0:
                            matrix.direction = new Vector3(element, matrix.direction.y, matrix.direction.z); break;
                        case 1:
                            matrix.direction = new Vector3(matrix.direction.x, element, matrix.direction.z); break;
                        case 2:
                            matrix.direction = new Vector3(matrix.direction.x, matrix.direction.y, element); break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(x));
                    }
                    break;
            }
        }
    }
}