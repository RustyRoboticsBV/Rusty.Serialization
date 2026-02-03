#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot rect2 converter.
    /// </summary>
    public class Rect2Converter : VectorConverter<Rect2, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Rect2 vector, int index)
        {
            switch (index)
            {
                case 0: return vector.Position.X;
                case 1: return vector.Position.Y;
                case 2: return vector.Size.X;
                case 3: return vector.Size.Y;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void SetElementAt(ref Rect2 vector, int index, ref float element)
        {
            switch (index)
            {
                case 0: vector.Position = new(element, vector.Position.Y); break;
                case 1: vector.Position = new(vector.Position.X, element); break;
                case 2: vector.Size = new(element, vector.Size.Y); break;
                case 3: vector.Size = new(vector.Size.X, element); break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
#endif