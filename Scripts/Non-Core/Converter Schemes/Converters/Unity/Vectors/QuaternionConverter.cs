#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnttyEngine.Quaternion converter.
    /// </summary>
    public sealed class QuaternionConverter : VectorConverter<Quaternion, float>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override float GetAt(ref Quaternion obj, int index)
        {
            switch (index)
            {
                case 0: return obj.x;
                case 1: return obj.y;
                case 2: return obj.z;
                case 3: return obj.w;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Quaternion obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.x = value; break;
                case 1: obj.y = value; break;
                case 2: obj.z = value; break;
                case 3: obj.w = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif