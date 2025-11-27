using System;
using System.Numerics;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Numerics.Matrix4x4 converter.
    /// </summary>
    public sealed class Matrix4x4Converter : VectorConverter<Matrix4x4, float>
    {
        /* Protected properties. */
        protected override int Length => 16;

        /* Protected methods. */
        protected override float GetAt(ref Matrix4x4 obj, int index)
        {
            switch (index)
            {
                case 0: return obj.M11;
                case 1: return obj.M21;
                case 2: return obj.M31;
                case 3: return obj.M41;
                case 4: return obj.M12;
                case 5: return obj.M22;
                case 6: return obj.M32;
                case 7: return obj.M42;
                case 8: return obj.M13;
                case 9: return obj.M23;
                case 10: return obj.M33;
                case 11: return obj.M43;
                case 12: return obj.M14;
                case 13: return obj.M24;
                case 14: return obj.M34;
                case 15: return obj.M44;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Matrix4x4 obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.M11 = value; break;
                case 1: obj.M21 = value; break;
                case 2: obj.M31 = value; break;
                case 3: obj.M41 = value; break;
                case 4: obj.M12 = value; break;
                case 5: obj.M22 = value; break;
                case 6: obj.M32 = value; break;
                case 7: obj.M42 = value; break;
                case 8: obj.M13 = value; break;
                case 9: obj.M23 = value; break;
                case 10: obj.M33 = value; break;
                case 11: obj.M43 = value; break;
                case 12: obj.M14 = value; break;
                case 13: obj.M24 = value; break;
                case 14: obj.M34 = value; break;
                case 15: obj.M44 = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}