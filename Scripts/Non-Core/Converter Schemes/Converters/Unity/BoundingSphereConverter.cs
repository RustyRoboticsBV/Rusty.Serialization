#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.BoundingSphere converter.
    /// </summary>
    public sealed class BoundingSphereConverter : VectorConverter<BoundingSphere, float>
    {
        /* Protected properties. */
        protected override int Length => 4;

        /* Protected methods. */
        protected override float GetAt(ref BoundingSphere obj, int index)
        {
            switch (index)
            {
                case 0: return obj.position.x;
                case 1: return obj.position.y;
                case 2: return obj.position.z;
                case 3: return obj.radius;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref BoundingSphere obj, int index, float value)
        {
            switch (index)
            {
                case 0: obj.position = new(value, obj.position.y, obj.position.z); break;
                case 1: obj.position = new(obj.position.x, value, obj.position.z); break;
                case 2: obj.position = new(obj.position.x, obj.position.y, value); break;
                case 3: obj.radius = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif