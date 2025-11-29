using System;
using System.Numerics;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Numerics.Vector2 converter.
    /// </summary>
    public sealed class Vector2Converter : VectorConverter<Vector2, float>
    {
        /* Protected properties. */
        protected override int Length => 2;

        /* Protected methods. */
        protected override float GetAt(ref Vector2 obj, int index)
        {
            switch (index)
            {
                case 0: return obj.X;
                case 1: return obj.Y;
                default: throw new ArgumentException($"Bad index {index}.");
            }
        }

        protected override void SetAt(ref Vector2 obj, int index, float value)
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