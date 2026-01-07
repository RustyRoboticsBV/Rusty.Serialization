#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity plane converter.
    /// </summary>
    public class PlaneConverter : VectorConverter<Plane, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Plane vector, int index)
        {
            switch (index)
            {
                case 0: return vector.normal.x;
                case 1: return vector.normal.y;
                case 2: return vector.normal.z;
                case 3: return vector.distance;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Plane vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.normal = new Vector3(element, vector.normal.y, vector.normal.z); break;
                case 1: vector.normal = new Vector3(vector.normal.x, element, vector.normal.z); break;
                case 2: vector.normal = new Vector3(vector.normal.x, vector.normal.y, element); break;
                case 3: vector.distance = element; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif