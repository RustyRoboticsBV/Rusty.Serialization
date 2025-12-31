#if NETCOREAPP2_0_OR_GREATER
using System;
using System.Drawing;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET int size converter.
    /// </summary>
    public class SizeConverter : VectorConverter<Size, int>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override int GetElementAt(ref Size vector, int index)
        {
            switch (index)
            {
                case 0: return vector.Width;
                case 1: return vector.Height;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Size vector, int index, ref int element)
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