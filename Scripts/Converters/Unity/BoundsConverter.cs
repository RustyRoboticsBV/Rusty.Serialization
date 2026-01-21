#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity bounds converter.
    /// </summary>
    public class BoundsConverter : VectorConverter<Bounds, float>
    {
        /* Protected method. */
        protected override int GetLength() => 6;

        protected override float GetElementAt(ref Bounds bounds, int index)
        {
            switch (index)
            {
                case 0: return bounds.min.x;
                case 1: return bounds.min.y;
                case 2: return bounds.min.z;
                case 3: return bounds.max.x;
                case 4: return bounds.max.y;
                case 5: return bounds.max.z;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Bounds bounds, int index, ref float element)
        {
            switch (index)
            {
                case 0: bounds.min = new Vector3(element, bounds.min.y, bounds.min.z); break;
                case 1: bounds.min = new Vector3(bounds.min.x, element, bounds.min.z); break;
                case 2: bounds.min = new Vector3(bounds.min.x, bounds.min.y, element); break;
                case 3: bounds.max = new Vector3(element, bounds.max.y, bounds.max.z); break;
                case 4: bounds.max = new Vector3(bounds.max.x, element, bounds.max.z); break;
                case 5: bounds.max = new Vector3(bounds.max.x, bounds.max.y, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif