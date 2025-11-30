#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.RectInt converter.
    /// </summary>
    public sealed class RectIntConverter : VectorConverter<RectInt, Vector2Int>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector2Int GetAt(ref RectInt obj, int index)
        {
            switch (index)
            {
                case 0: return obj.position;
                case 1: return obj.size;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref RectInt obj, int index, Vector2Int value)
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