#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot basis converter.
    /// </summary>
    public class BasisConverter : MatrixConverter<Basis, float>
    {
        /* Protected method. */
        protected override int GetLength() => 9;
        protected override int GetWidth() => 3;

        protected override float GetElementAt(ref Basis matrix, int x, int y)
        {
            return matrix[x, y];
        }

        protected override void SetElementAt(ref Basis matrix, int x, int y, ref float element)
        {
            matrix[x, y] = element;
        }
    }
}
#endif