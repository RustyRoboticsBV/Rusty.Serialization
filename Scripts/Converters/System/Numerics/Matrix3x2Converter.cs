using System;
using System.Numerics;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Numerics.Matrix3x2 converter.
    /// </summary>
    public sealed class Matrix3x2Converter : VectorConverter<Matrix3x2, float>
    {
        /* Protected properties. */
        protected override int Length => 6;

        /* Protected methods. */
        protected override float GetAt(ref Matrix3x2 obj, int index)
        {
            switch (index)
            {
                case 0: return obj.M11;
                case 1: return obj.M21;
                case 2: return obj.M31;
                case 3: return obj.M12;
                case 4: return obj.M22;
                case 5: return obj.M32;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Matrix3x2 obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.M11 = value; break;
                case 1: obj.M21 = value; break;
                case 2: obj.M31 = value; break;
                case 3: obj.M12 = value; break;
                case 4: obj.M22 = value; break;
                case 5: obj.M32 = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}