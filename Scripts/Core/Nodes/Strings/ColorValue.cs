using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A color node value.
    /// </summary>
    public readonly struct ColorValue : IEquatable<ColorValue>
    {
        /* Fields */
        public readonly byte r;
        public readonly byte g;
        public readonly byte b;
        public readonly byte a;

        /* Constructors */
        public ColorValue(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /* Conversion operators */
        public static implicit operator ColorValue((byte, byte, byte, byte) value)
            => new ColorValue(value.Item1, value.Item2, value.Item3, value.Item4);

        /* Public methods */
        public override string ToString() => $"({r}, {g}, {b}, {a})";
        public override int GetHashCode() => HashCode.Combine(r, g, b, a);
        public override bool Equals(object obj) => obj is ColorValue other && Equals(other);
        public bool Equals(ColorValue other) => r == other.r && g == other.g && b == other.b && a == other.a;
    }
}