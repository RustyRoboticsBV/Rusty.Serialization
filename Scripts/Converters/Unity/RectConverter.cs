#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity rect converter.
    /// </summary>
    public class RectConverter : VectorConverter<Rect, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Rect vector, int index)
        {
            switch (index)
            {
                case 0: return vector.position.x;
                case 1: return vector.position.y;
                case 2: return vector.size.x;
                case 3: return vector.size.y;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Rect vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.position = new Vector2(element, vector.position.y); break;
                case 1: vector.position = new Vector2(vector.position.x, element); break;
                case 2: vector.size = new Vector2(element, vector.size.y); break;
                case 3: vector.size = new Vector2(vector.size.x, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif