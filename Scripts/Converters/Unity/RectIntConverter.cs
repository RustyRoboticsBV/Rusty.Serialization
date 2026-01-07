#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity integer rect converter.
    /// </summary>
    public class RectIntConverter : VectorConverter<RectInt, int>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override int GetElementAt(ref RectInt vector, int index)
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

        protected override void SetElementAt(ref RectInt vector, int index, ref int element)
        {
            switch (index)
            {
                case 0: vector.position = new Vector2Int(element, vector.position.y); break;
                case 1: vector.position = new Vector2Int(vector.position.x, element); break;
                case 2: vector.size = new Vector2Int(element, vector.size.y); break;
                case 3: vector.size = new Vector2Int(vector.size.x, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif