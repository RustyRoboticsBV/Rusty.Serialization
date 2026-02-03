#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot int vector4 converter.
    /// </summary>
    public class Vector4IConverter : VectorConverter<Vector4I, int>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override int GetElementAt(ref Vector4I vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector4I vector, int index, ref int element)
        {
            vector[index] = element;
        }
    }
}
#endif