using System;
using System.Numerics;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET complex converter.
    /// </summary>
    public class ComplexConverter : VectorConverter<Complex, double>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override double GetElementAt(ref Complex vector, int index)
        {
            switch (index)
            {
                case 0: return vector.Real;
                case 1: return vector.Imaginary;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Complex vector, int index, ref double element)
        {
            switch (index)
            {
                case 0: vector = new Complex(element, vector.Imaginary); break;
                case 1: vector = new Complex(vector.Real, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}