#if NETCOREAPP2_0_OR_GREATER
using System;
using System.Drawing;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET float size converter.
    /// </summary>
    public class SizeFConverter : VectorConverter<SizeF, float>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override float GetElementAt(ref SizeF vector, int index)
        {
            switch (index)
            {
                case 0: return vector.Width;
                case 1: return vector.Height;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref SizeF vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.Width = element; break;
                case 1: vector.Height = element; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif