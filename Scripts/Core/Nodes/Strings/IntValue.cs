using System;
using System.Numerics;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An int node value.
    /// </summary>
    public readonly struct IntValue : IEquatable<IntValue>
    {
        /* Fields */
        public readonly BigInteger value;

        /* Constructors */
        public IntValue(BigInteger value)
        {
            this.value = value;
        }

        /* Conversion operators */
        public static implicit operator IntValue(BigInteger value) => new IntValue(value);

        /* Comparison operators. */
        public static bool operator ==(IntValue a, IntValue b) => a.Equals(b);
        public static bool operator !=(IntValue a, IntValue b) => !a.Equals(b);

        /* Public methods */
        public override string ToString() => value.ToString();
        public override int GetHashCode() => value.GetHashCode();
        public override bool Equals(object obj) => obj is IntValue other && Equals(other);
        public bool Equals(IntValue other) => value.Equals(other.value);
    }
}