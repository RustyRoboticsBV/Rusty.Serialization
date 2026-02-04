using System;
using System.Numerics;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An integer value of arbitrary precision.
    /// </summary>
    public readonly struct IntValue : IEquatable<IntValue>
    {
        /* Fields */
        public readonly BigInteger value;

        /* Public properties. */
        public bool IsZero => value == 0;
        public bool IsOne => value == 1;

        /* Constructors */
        public IntValue(BigInteger value)
        {
            this.value = value;
        }

        /* Conversion operators */
        public static implicit operator IntValue(byte value) => new IntValue(value);
        public static implicit operator IntValue(sbyte value) => new IntValue(value);
        public static implicit operator IntValue(short value) => new IntValue(value);
        public static implicit operator IntValue(ushort value) => new IntValue(value);
        public static implicit operator IntValue(int value) => new IntValue(value);
        public static implicit operator IntValue(long value) => new IntValue(value);
        public static implicit operator IntValue(uint value) => new IntValue(value);
        public static implicit operator IntValue(ulong value) => new IntValue(value);
        public static implicit operator IntValue(BigInteger value) => new IntValue(value);
        public static explicit operator BigInteger(IntValue value) => value.value;
        public static explicit operator byte(IntValue value) => (byte)value.value;
        public static explicit operator sbyte(IntValue value) => (sbyte)value.value;
        public static explicit operator short(IntValue value) => (short)value.value;
        public static explicit operator ushort(IntValue value) => (ushort)value.value;
        public static explicit operator int(IntValue value) => (int)value.value;
        public static explicit operator uint(IntValue value) => (uint)value.value;
        public static explicit operator long(IntValue value) => (long)value.value;
        public static explicit operator ulong(IntValue value) => (ulong)value.value;

        /* Arithmetic operators. */
        public static IntValue operator -(IntValue v) => new IntValue(-v.value);

        /* Comparison operators. */
        public static bool operator ==(IntValue a, IntValue b) => a.value == b.value;
        public static bool operator !=(IntValue a, IntValue b) => a.value != b.value;
        public static bool operator >(IntValue a, IntValue b) => a.value > b.value;
        public static bool operator <(IntValue a, IntValue b) => a.value < b.value;
        public static bool operator >=(IntValue a, IntValue b) => a.value >= b.value;
        public static bool operator <=(IntValue a, IntValue b) => a.value <= b.value;

        /* Public methods */
        public override string ToString() => value.ToString();
        public override int GetHashCode() => value.GetHashCode();
        public override bool Equals(object obj) => obj is IntValue other && Equals(other);
        public bool Equals(IntValue other) => value.Equals(other.value);

        /// <summary>
        /// Parse an integer string.
        /// </summary>
        public static IntValue Parse(ReadOnlySpan<char> text)
        {
            try
            {
                return new IntValue(BigInteger.Parse(text));
            }
            catch (FormatException)
            {
                throw new FormatException($"Not a valid integer: {new string(text)}.");
            }
        }
    }
}