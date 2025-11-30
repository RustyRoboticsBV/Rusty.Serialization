#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Plane converter.
    /// </summary>
    public sealed class PlaneConverter : VectorConverter<Plane, float>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override float GetAt(ref Plane obj, int index)
        {
            switch (index)
            {
                case 0: return obj.normal.x;
                case 1: return obj.normal.y;
                case 2: return obj.normal.z;
                case 3: return obj.distance;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Plane obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.normal = new(value, obj.normal.y, obj.normal.z); break;
                case 1: obj.normal = new(obj.normal.x, value, obj.normal.z); break;
                case 2: obj.normal = new(obj.normal.x, obj.normal.y, value); break;
                case 3: obj.distance = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif