#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot 2D transform converter.
    /// </summary>
    public class Transform2DConverter : MatrixConverter<Transform2D, float>
    {
        /* Protected method. */
        protected override int GetLength() => 6;
        protected override int GetWidth() => 3;

        protected override float GetElementAt(ref Transform2D matrix, int x, int y)
        {
            return matrix[x, y];
        }

        protected override void SetElementAt(ref Transform2D matrix, int x, int y, ref float element)
        {
            matrix[x, y] = element;
        }
    }
}
#endif