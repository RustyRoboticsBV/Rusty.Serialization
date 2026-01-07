#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity bounds converter.
    /// </summary>
    public class BoundsConverter : VectorConverter<Bounds, float>
    {
        /* Protected method. */
        protected override int GetLength() => 6;

        protected override float GetElementAt(ref Bounds vector, int index)
        {
            switch (index)
            {
                case 0: return vector.min.x;
                case 1: return vector.min.y;
                case 2: return vector.min.z;
                case 3: return vector.max.x;
                case 4: return vector.max.y;
                case 5: return vector.max.z;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Bounds vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.min = new Vector3(element, vector.min.y, vector.min.z); break;
                case 1: vector.min = new Vector3(vector.min.x, element, vector.min.z); break;
                case 2: vector.min = new Vector3(vector.min.x, vector.min.y, element); break;
                case 3: vector.max = new Vector3(element, vector.max.y, vector.max.z); break;
                case 4: vector.max = new Vector3(vector.max.x, element, vector.max.z); break;
                case 5: vector.max = new Vector3(vector.max.x, vector.max.y, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif