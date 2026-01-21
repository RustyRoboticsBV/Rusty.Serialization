#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity integer bounds converter.
    /// </summary>
    public class BoundsIntConverter : VectorConverter<BoundsInt, int>
    {
        /* Protected method. */
        protected override int GetLength() => 6;

        protected override int GetElementAt(ref BoundsInt bounds, int index)
        {
            switch (index)
            {
                case 0: return bounds.position.x;
                case 1: return bounds.position.y;
                case 2: return bounds.position.z;
                case 3: return bounds.size.x;
                case 4: return bounds.size.y;
                case 5: return bounds.size.z;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref BoundsInt bounds, int index, ref int element)
        {
            switch (index)
            {
                case 0: bounds.position = new Vector3Int(element, bounds.position.y, bounds.position.z); break;
                case 1: bounds.position = new Vector3Int(bounds.position.x, element, bounds.position.z); break;
                case 2: bounds.position = new Vector3Int(bounds.position.x, bounds.position.y, element); break;
                case 3: bounds.size = new Vector3Int(element, bounds.size.y, bounds.size.z); break;
                case 4: bounds.size = new Vector3Int(bounds.size.x, element, bounds.size.z); break;
                case 5: bounds.size = new Vector3Int(bounds.size.x, bounds.size.y, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif