using System;

namespace Rusty.Serialization.Core.Serializers.Utils
{
    public readonly struct EscapeCharacter
    {
        public readonly char Source { get; }
        public readonly string Escaped { get; }

        public EscapeCharacter(char source, string escaped)
        {
            Source = source;
            Escaped = escaped;
        }
    }

    public readonly struct AllowedCharacterRange
    {
        public readonly int Start { get; }
        public readonly int End { get; }

        public AllowedCharacterRange(int start, int end)
        {
            if (end < start)
                throw new ArgumentException($"end = {end} is smaller than start = {start}");
            Start = start;
            End = end;
        }

        public bool Has(int chr)
        {
            return chr >= Start && chr <= End;
        }
    }
}