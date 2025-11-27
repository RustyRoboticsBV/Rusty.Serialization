#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Vector2I converter.
    /// </summary>
    public sealed class Vector2IConverter : VectorConverter<Vector2I, int>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override int GetAt(ref Vector2I obj, int index)
        {
            switch (index)
            {
                case 0: return obj.X;
                case 1: return obj.Y;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Vector2I obj, int index, int value)
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