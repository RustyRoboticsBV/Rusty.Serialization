using System.Numerics;
using Rusty.Serialization.Core.Converters;

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
            return matrix[x, y];
        }

        protected override void SetElementAt(ref Matrix4x4 matrix, int x, int y, ref float element)
        {
            matrix[x, y] = element;
        }
    }
}