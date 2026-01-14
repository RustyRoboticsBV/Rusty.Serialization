using System;
using System.Globalization;

namespace Rusty.Serialization.Core.Serializers
{
    public readonly struct UnicodePair
    {
        /* Fields. */
        public readonly char High;
        public readonly char Low;

        /* Public properties. */
        public int Length => Low == '\0' ? 1 : 2;
        public int CodePoint => Low == '\0' ? High : ((High - 0xD800) << 10) + (Low - 0xDC00) + 0x10000;
        public string Hex => CodePoint.ToString("X", CultureInfo.InvariantCulture);

        /* Constructors. */
        public UnicodePair(char chr)
        {
            High = chr;
            Low = '\0';
        }

        public UnicodePair(char high, char low)
        {
            High = high;
            Low = low;
        }

        public UnicodePair(int codePoint)
        {
            if ((uint)codePoint > 0x10FFFF || (uint)(codePoint - 0xD800) < 0x800)
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

        public UnicodePair(string str, int index)
        {
            if (str is null)
                throw new ArgumentNullException(nameof(str));

            if ((uint)index >= (uint)str.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

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

        /* Public methods. */
        public override string ToString()
        {
            if (Low == '\0')
                return High.ToString();
            else
                return string.Concat(High, Low);
        }

        public void CopyTo(Span<char> destination)
        {
            destination[0] = High;
            if (Low != '\0')
                destination[1] = Low;
        }
    }
}