#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot 3D transform converter.
    /// </summary>
    public class Transform3DConverter : MatrixConverter<Transform3D, float>
    {
        /* Protected method. */
        protected override int GetLength() => 12;
        protected override int GetWidth() => 4;

        protected override float GetElementAt(ref Transform3D matrix, int x, int y)
        {
            return matrix[x, y];
        }

        protected override void SetElementAt(ref Transform3D matrix, int x, int y, ref float element)
        {
            matrix[x, y] = element;
        }
    }
}
#endif