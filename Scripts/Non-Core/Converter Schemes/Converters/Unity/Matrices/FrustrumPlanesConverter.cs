#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.FrustumPlanes converter.
    /// </summary>
    public sealed class FrustumPlanesConverter : VectorConverter<FrustumPlanes, float>
    {
        /* Protected properties. */
        protected override int Length => 6;

        /* Protected methods. */
        protected override float GetAt(ref FrustumPlanes obj, int index)
        {
            switch (index)
            {
                case 0: return obj.bottom;
                case 1: return obj.left;
                case 2: return obj.right;
                case 3: return obj.top;
                case 4: return obj.zFar;
                case 5: return obj.zNear;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref FrustumPlanes obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.bottom = value; break;
                case 1: obj.left = value; break;
                case 2: obj.right = value; break;
                case 3: obj.top = value; break;
                case 4: obj.zFar = value; break;
                case 5: obj.zNear = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif