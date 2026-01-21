using System;
using System.Numerics;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET 4x4 matrix converter.
    /// </summary>
    public class Matrix4x4Converter : MatrixConverter<Matrix4x4, float>
    {
        /* Protected method. */
        protected override int GetLength() => 16;
        protected override int GetWidth() => 4;

        protected override float GetElementAt(ref Matrix4x4 matrix, int x, int y)
        {
            switch (y)
            {
                case 0:
                    switch (x)
                    {
                        case 0: return matrix.M11;
                        case 1: return matrix.M12;
                        case 2: return matrix.M13;
                        case 3: return matrix.M14;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                case 1:
                    switch (x)
                    {
                        case 0: return matrix.M21;
                        case 1: return matrix.M22;
                        case 2: return matrix.M23;
                        case 3: return matrix.M24;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                case 2:
                    switch (x)
                    {
                        case 0: return matrix.M31;
                        case 1: return matrix.M32;
                        case 2: return matrix.M33;
                        case 3: return matrix.M34;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                case 3:
                    switch (x)
                    {
                        case 0: return matrix.M41;
                        case 1: return matrix.M42;
                        case 2: return matrix.M43;
                        case 3: return matrix.M44;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                default:
                    throw new IndexOutOfRangeException(nameof(y));
            }
        }

        protected override void SetElementAt(ref Matrix4x4 matrix, int x, int y, ref float element)
        {
            switch (y)
            {
                case 0:
                    switch (x)
                    {
                        case 0: matrix.M11 = element; break;
                        case 1: matrix.M12 = element; break;
                        case 2: matrix.M13 = element; break;
                        case 3: matrix.M14 = element; break;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                    break;

                case 1:
                    switch (x)
                    {
                        case 0: matrix.M21 = element;break;
                        case 1: matrix.M22 = element; break;
                        case 2: matrix.M23 = element; break;
                        case 3: matrix.M24 = element; break;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                    break;

                case 2:
                    switch (x)
                    {
                        case 0: matrix.M31 = element; break;
                        case 1: matrix.M32 = element; break;
                        case 2: matrix.M33 = element; break;
                        case 3: matrix.M34 = element; break;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                    break;

                case 3:
                    switch (x)
                    {
                        case 0: matrix.M41 = element; break;
                        case 1: matrix.M42 = element; break;
                        case 2: matrix.M43 = element; break;
                        case 3: matrix.M44 = element; break;
                        default: throw new IndexOutOfRangeException(nameof(x));
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException(nameof(y));
            }
        }
    }
}