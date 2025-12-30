#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot projection converter.
    /// </summary>
    public class ProjectionConverter : MatrixConverter<Projection, float>
    {
        /* Protected method. */
        protected override int GetLength() => 16;
        protected override int GetWidth() => 4;

        protected override float GetElementAt(ref Projection matrix, int x, int y)
        {
            return matrix[x, y];
        }

        protected override void SetElementAt(ref Projection matrix, int x, int y, ref float element)
        {
            matrix[x, y] = element;
        }
    }
}
#endif