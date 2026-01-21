#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity integer vector3 converter.
    /// </summary>
    public class Vector3IntConverter : VectorConverter<Vector3Int, int>
    {
        /* Protected method. */
        protected override int GetLength() => 3;

        protected override int GetElementAt(ref Vector3Int vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector3Int vector, int index, ref int element)
        {
            vector[index] = element;
        }
    }
}
#endif