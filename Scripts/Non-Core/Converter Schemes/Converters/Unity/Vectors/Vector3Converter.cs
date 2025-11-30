#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Vector3 converter.
    /// </summary>
    public sealed class Vector3Converter : VectorConverter<Vector3, float>
    {
        /* Protected properties. */
        protected override int Length => 3;

        /* Protected methods. */
        protected override float GetAt(ref Vector3 obj, int index)
        {
            switch (index)
            {
                case 0: return obj.x;
                case 1: return obj.y;
                case 2: return obj.z;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Vector3 obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.x = value; break;
                case 1: obj.y = value; break;
                case 2: obj.z = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif