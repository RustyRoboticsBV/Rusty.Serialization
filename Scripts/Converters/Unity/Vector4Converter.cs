#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity vector4 converter.
    /// </summary>
    public class Vector4Converter : VectorConverter<Vector4, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Vector4 vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector4 vector, int index, ref float element)
        {
            vector[index] = element;
        }
    }
}
#endif