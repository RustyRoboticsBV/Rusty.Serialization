using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    public readonly struct CharValue : IEquatable<char>, IEquatable<int>, IEquatable<CharValue>
    {
        /* Fields. */
        public readonly char High;
        public readonly char Low;

        /* Public properties. */
        public int Length => Low == '\0' ? 1 : 2;
        public int CodePoint => Low == '\0' ? High : ((High - 0xD800) << 10) + (Low - 0xDC00) + 0x10000;
        public string Hex => CodePoint.ToString("X", CultureInfo.InvariantCulture);

        /* Constructors. */
        public CharValue(char chr)
        {
            High = chr;
            Low = '\0';
        }

        public CharValue(char high, char low)
        {
            High = high;
            Low = low;
        }

        public CharValue(int codePoint)
        {
            if ((uint)codePoint > 0x10FFFF /*|| (uint)(codePoint - 0xD800) < 0x800*/) // TODO: determine what to do with lone surrogate pairs.
                throw new ArgumentOutOfRangeException(nameof(codePoint));

            if (codePoint <= 0xFFFF)
            {
                High = (char)codePoint;
                Low = '\0';
            }
            else
            {
                codePoint -= 0x10000;
                High = (char)((codePoint >> 10) + 0xD800);
                Low = (char)((codePoint & 0x3FF) + 0xDC00);
            }
        }

        public CharValue(string str, int index)
        {
            if (str is null)
                throw new ArgumentNullException(nameof(str));

            if ((uint)index >= (uint)str.Length)
                throw new ArgumentOutOfRangeException(nameof(index), $"{index} / {str.Length}");

            if (index + 1 < str.Length && char.IsHighSurrogate(str[index]) && char.IsLowSurrogate(str[index + 1]))
            {
                High = str[index];
                Low = str[index + 1];
            }
            else
            {
                High = str[index];
                Low = '\0';
            }
        }

        public CharValue(string str) : this(str, 0) { }

        /* Conversion operators. */
        public static implicit operator CharValue(string str) => new CharValue(str);
        public static implicit operator CharValue(char chr) => new CharValue(chr);
        public static implicit operator CharValue(int codePoint) => new CharValue(codePoint);
        public static explicit operator char(CharValue pair) => pair.High;

        /* Comparison operators. */
        public static bool operator ==(CharValue a, CharValue b) => a.Equals(b);
        public static bool operator !=(CharValue a, CharValue b) => !a.Equals(b);
        public static bool operator >(CharValue a, CharValue b) => a.CodePoint > b.CodePoint;
        public static bool operator <(CharValue a, CharValue b) => a.CodePoint < b.CodePoint;
        public static bool operator >=(CharValue a, CharValue b) => a.CodePoint >= b.CodePoint;
        public static bool operator <=(CharValue a, CharValue b) => a.CodePoint <= b.CodePoint;

        /* Public methods. */
        public override string ToString()
        {
            if (Low == '\0')
                return High.ToString();
            else
                return string.Concat(High, Low);
        }
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case char c: return Equals(c);
                case int i: return Equals(i);
                case CharValue pair: return Equals(pair);
                default: return base.Equals(obj);
            }
        }
        public bool Equals(char c) => High == c && Low == '\0';
        public bool Equals(int codePoint) => CodePoint == codePoint;
        public bool Equals(CharValue other) => High == other.High && Low == other.Low;
        public override int GetHashCode() => HashCode.Combine(High, Low);

        public void CopyTo(Span<char> destination)
        {
            if (destination.Length < Length)
                throw new ArgumentException("Destination span is too small.", nameof(destination));

            destination[0] = High;
            if (Low != '\0')
                destination[1] = Low;
        }
    }
}