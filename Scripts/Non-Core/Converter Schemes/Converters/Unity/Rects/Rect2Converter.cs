#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Rect converter.
    /// </summary>
    public sealed class RectConverter : VectorConverter<Rect, Vector2>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector2 GetAt(ref Rect obj, int index)
        {
            switch (index)
            {
                case 0: return obj.position;
                case 1: return obj.size;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Rect obj, int index, Vector2 value)
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