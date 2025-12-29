using System.Numerics;
using Rusty.Serialization.Core.Converters;

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
            return matrix[x, y];
        }

        protected override void SetElementAt(ref Matrix3x2 matrix, int x, int y, ref float element)
        {
            matrix[x, y] = element;
        }
    }
}