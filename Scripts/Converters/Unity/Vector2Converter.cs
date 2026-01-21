#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity vector2 converter.
    /// </summary>
    public class Vector2Converter : VectorConverter<Vector2, float>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override float GetElementAt(ref Vector2 vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector2 vector, int index, ref float element)
        {
            vector[index] = element;
        }
    }
}
#endif