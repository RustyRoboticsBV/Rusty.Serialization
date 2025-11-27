using System;
using System.Numerics;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Numerics.Complex converter.
    /// </summary>
    public sealed class ComplexConverter : VectorConverter<Complex, double>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override double GetAt(ref Complex obj, int index)
        {
            switch (index)
            {
                case 0: return obj.Real;
                case 1: return obj.Imaginary;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Complex obj, int index, double value)
        {
            switch (index)
            {
                case 0: obj = new(value, obj.Imaginary); break;
                case 1: obj = new(obj.Real, value); break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}