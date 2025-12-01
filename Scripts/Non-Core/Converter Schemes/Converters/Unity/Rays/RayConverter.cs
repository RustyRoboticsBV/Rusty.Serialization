#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Ray converter.
    /// </summary>
    public sealed class RayConverter : VectorConverter<Ray, Vector3>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector3 GetAt(ref Ray obj, int index)
        {
            switch (index)
            {
                case 0: return obj.origin;
                case 1: return obj.direction;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Ray obj, int index, Vector3 value)
        {
            switch (index)
            {
                case 0: obj.origin = value; break;
                case 1: obj.direction = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif