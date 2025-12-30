#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot vector3 converter.
    /// </summary>
    public class Vector3Converter : VectorConverter<Vector3, float>
    {
        /* Protected method. */
        protected override int GetLength() => 3;

        protected override float GetElementAt(ref Vector3 vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector3 vector, int index, ref float element)
        {
            vector[index] = element;
        }
    }
}
#endif