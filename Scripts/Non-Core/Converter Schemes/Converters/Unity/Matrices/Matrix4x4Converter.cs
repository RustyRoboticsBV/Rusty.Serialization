#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Matrix4x4 converter.
    /// </summary>
    public sealed class Matrix4x4Converter : VectorConverter<Matrix4x4, Vector4>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override Vector4 GetAt(ref Matrix4x4 obj, int index)
        {
            switch (index)
            {
                case 0: return obj.GetColumn(0);
                case 1: return obj.GetColumn(1);
                case 2: return obj.GetColumn(2);
                case 3: return obj.GetColumn(3);
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Matrix4x4 obj, int index, Vector4 value)
        {
            switch (index)
            {
                case 0: obj.SetColumn(0, value); break;
                case 1: obj.SetColumn(1, value); break;
                case 2: obj.SetColumn(2, value); break;
                case 3: obj.SetColumn(3, value); break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif