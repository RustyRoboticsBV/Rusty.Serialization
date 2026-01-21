#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity quaternion converter.
    /// </summary>
    public class QuaternionConverter : VectorConverter<Quaternion, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Quaternion vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Quaternion vector, int index, ref float element)
        {
            vector[index] = element;
        }
    }
}
#endif