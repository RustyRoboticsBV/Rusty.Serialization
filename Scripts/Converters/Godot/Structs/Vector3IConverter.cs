#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot int vector3 converter.
    /// </summary>
    public class Vector3IConverter : VectorConverter<Vector3I, int>
    {
        /* Protected method. */
        protected override int GetLength() => 3;

        protected override int GetElementAt(ref Vector3I vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector3I vector, int index, ref int element)
        {
            vector[index] = element;
        }
    }
}
#endif