using System;
using System.Numerics;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET 3x2 matrix converter.
    /// </summary>
    public class Matrix3x2Converter : MatrixConverter<Matrix3x2, float>
    {
        /* Protected method. */
        protected override int GetLength() => 6;
        protected override int GetWidth() => 3;

        protected override float GetElementAt(ref Matrix3x2 matrix, int x, int y)
        {
            switch (y)
            {
                case 0:
                    switch (x)
                    {
                        case 0: return matrix.M11;
                        case 1: return matrix.M12;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                case 1:
                    switch (x)
                    {
                        case 0: return matrix.M21;
                        case 1: return matrix.M22;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                case 2:
                    switch (x)
                    {
                        case 0: return matrix.M31;
                        case 1: return matrix.M32;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                default:
                    throw new IndexOutOfRangeException(nameof(y));
            }
        }

        protected override void SetElementAt(ref Matrix3x2 matrix, int x, int y, ref float element)
        {
            switch (y)
            {
                case 0:
                    switch (x)
                    {
                        case 0: matrix.M11 = element; break;
                        case 1: matrix.M12 = element; break;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                    break;

                case 1:
                    switch (x)
                    {
                        case 0: matrix.M21 = element; break;
                        case 1: matrix.M22 = element; break;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                    break;

                case 2:
                    switch (x)
                    {
                        case 0: matrix.M31 = element; break;
                        case 1: matrix.M32 = element; break;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException(nameof(y));
            }
        }
    }
}