#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.RangeInt converter.
    /// </summary>
    public sealed class RangeIntConverter : VectorConverter<RangeInt, int>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override int GetAt(ref RangeInt obj, int index)
        {
            switch (index)
            {
                case 0: return obj.start;
                case 1: return obj.length;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref RangeInt obj, int index, int value)
        {
            switch (index)
            {
                case 0: obj.start = value; break;
                case 1: obj.length = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif