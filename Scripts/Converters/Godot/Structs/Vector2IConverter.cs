#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot int vector2 converter.
    /// </summary>
    public class Vector2IConverter : VectorConverter<Vector2I, int>
    {
        /* Protected method. */
        protected override int GetLength() => 2;

        protected override int GetElementAt(ref Vector2I vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Vector2I vector, int index, ref int element)
        {
            vector[index] = element;
        }
    }
}
#endif