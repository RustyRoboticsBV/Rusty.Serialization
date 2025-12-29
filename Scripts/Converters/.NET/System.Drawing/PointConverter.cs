using System;
using System.Drawing;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET int point converter.
    /// </summary>
    public class PointConverter : VectorConverter<Point, int>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override int GetElementAt(ref Point vector, int index)
        {
            switch (index)
            {
                case 0: return vector.X;
                case 1: return vector.Y;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Point vector, int index, ref int element)
        {
            switch (index)
            {
                case 0: vector.X = element; break;
                case 1: vector.Y = element; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}