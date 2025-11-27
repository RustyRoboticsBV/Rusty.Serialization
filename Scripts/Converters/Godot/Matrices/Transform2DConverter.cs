#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Transform2D converter.
    /// </summary>
    public sealed class Transform2DConverter : VectorConverter<Transform2D, Vector2>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector2 GetAt(ref Transform2D obj, int index)
        {
            switch (index)
            {
                case 0: return obj.X;
                case 1: return obj.Y;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Transform2D obj, int index, Vector2 value)
        {
            switch (index)
            {
                case 0: obj.X = value; break;
                case 1: obj.Y = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif