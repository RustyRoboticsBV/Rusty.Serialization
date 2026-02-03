#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot vector4 converter.
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