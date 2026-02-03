#if NETCOREAPP2_0_OR_GREATER
using System;
using System.Drawing;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET rectangle converter.
    /// </summary>
    public class RectangleConverter : VectorConverter<Rectangle, int>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override int GetElementAt(ref Rectangle vector, int index)
        {
            switch (index)
            {
                case 0: return vector.X;
                case 1: return vector.Y;
                case 2: return vector.Width;
                case 3: return vector.Height;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Rectangle vector, int index, ref int element)
        {
            switch (index)
            {
                case 0: vector.X = element; break;
                case 1: vector.Y = element; break;
                case 2: vector.Width = element; break;
                case 3: vector.Height = element; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif