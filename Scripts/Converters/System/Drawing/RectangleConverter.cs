using System;
using Rusty.Serialization.Core.Converters;
using System.Drawing;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Drawing.Rectangle converter.
    /// </summary>
    public sealed class RectangleConverter : VectorConverter<Rectangle, int>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override int GetAt(ref Rectangle obj, int index)
        {
            switch (index)
            {
                case 0: return obj.X;
                case 1: return obj.Y;
                case 2: return obj.Width;
                case 3: return obj.Height;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Rectangle obj, int index, int value)
        {
            switch (index)
            {
                case 0: obj.X = value; break;
                case 1: obj.Y = value; break;
                case 2: obj.Width = value; break;
                case 3: obj.Height = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}