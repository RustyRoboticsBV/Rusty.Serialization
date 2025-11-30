#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Bounds converter.
    /// </summary>
    public sealed class BoundsConverter : VectorConverter<Bounds, Vector3>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector3 GetAt(ref Bounds obj, int index)
        {
            switch (index)
            {
                case 0: return obj.center;
                case 1: return obj.size;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Bounds obj, int index, Vector3 value)
        {
            switch (index)
            {
                case 0: obj.center = value; break;
                case 1: obj.size = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif