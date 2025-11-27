#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Rect2I converter.
    /// </summary>
    public sealed class Rect2IConverter : VectorConverter<Rect2I, Vector2I>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override Vector2I GetAt(ref Rect2I obj, int index)
        {
            switch (index)
            {
                case 0: return obj.Position;
                case 1: return obj.Size;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Rect2I obj, int index, Vector2I value)
        {
            switch (index)
            {
                case 0: obj.Position = value; break;
                case 1: obj.Size = value; break;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }
    }
}
#endif