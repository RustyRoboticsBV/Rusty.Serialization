#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity bounding sphere converter.
    /// </summary>
    public class BoundingSphereConverter : VectorConverter<BoundingSphere, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref BoundingSphere vector, int index)
        {
            switch (index)
            {
                case 0: return vector.position.x;
                case 1: return vector.position.y;
                case 2: return vector.position.z;
                case 3: return vector.radius;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref BoundingSphere vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.position = new Vector3(element, vector.position.y, vector.position.z); break;
                case 1: vector.position = new Vector3(vector.position.x, element, vector.position.z); break;
                case 2: vector.position = new Vector3(vector.position.x, vector.position.y, element); break;
                case 3: vector.radius = element; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif