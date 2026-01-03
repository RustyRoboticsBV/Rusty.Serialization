#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity integer vector2 converter.
    /// </summary>
    public class Vector2IntConverter : VectorConverter<Vector2Int, int>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override int GetElementAt(ref Vector2Int vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector2Int vector, int index, ref int element)
        {
            vector[index] = element;
        }
    }
}
#endif