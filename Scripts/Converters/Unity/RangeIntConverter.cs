#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity integer range converter.
    /// </summary>
    public class RangeIntConverter : VectorConverter<RangeInt, int>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override int GetElementAt(ref RangeInt vector, int index)
        {
            switch (index)
            {
                case 0: return vector.start;
                case 1: return vector.length;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref RangeInt vector, int index, ref int element)
        {
            switch (index)
            {
                case 0: vector.start = element; break;
                case 1: vector.length = element; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif