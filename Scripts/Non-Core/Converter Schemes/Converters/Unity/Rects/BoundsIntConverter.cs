#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.BoundsInt converter.
    /// </summary>
    public sealed class BoundsIntConverter : VectorConverter<BoundsInt, Vector3Int>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector3Int GetAt(ref BoundsInt obj, int index)
        {
            switch (index)
            {
                case 0: return obj.position;
                case 1: return obj.size;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref BoundsInt obj, int index, Vector3Int value)
        {
            switch (index)
            {
                case 0: obj.position = value; break;
                case 1: obj.size = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif