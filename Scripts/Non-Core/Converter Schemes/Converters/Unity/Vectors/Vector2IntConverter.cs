#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Vector2Int converter.
    /// </summary>
    public sealed class Vector2IntConverter : VectorConverter<Vector2Int, int>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override int GetAt(ref Vector2Int obj, int index)
        {
            switch (index)
            {
                case 0: return obj.x;
                case 1: return obj.y;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Vector2Int obj, int index, int value)
        {
            switch (index)
            {
                case 0: obj.x = value; break;
                case 1: obj.y = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif