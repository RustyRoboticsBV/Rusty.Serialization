#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Ray2D converter.
    /// </summary>
    public sealed class Ray2DConverter : VectorConverter<Ray2D, Vector2>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector2 GetAt(ref Ray2D obj, int index)
        {
            switch (index)
            {
                case 0: return obj.origin;
                case 1: return obj.direction;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Ray2D obj, int index, Vector2 value)
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