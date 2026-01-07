using System;
using System.Text;

namespace Rusty.Serialization.Core.Serializers.Utils
{
    /// <summary>
    /// A method that converts/deconverts a unicode string.
    /// </summary>
    public delegate string UnicodeConverter(string src);
    /// <summary>
    /// If a substring starting at some index represents a Unicode sequence, return the length. Returns -1 if no Unicode sequence
    /// starts at the index.
    /// </summary>
    public delegate int UnicodeChecker(string str, int index);

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

    public static class StringFormatter
    {
        public static string Format(string str, string start, string end, EscapeCharacter[] escapeCharacters,
            AllowedCharacterRange[] allowedCharacters, UnicodeConverter unicodeFormatter)
        {
            StringBuilder formatter = new StringBuilder();

            // Write opening.
            formatter.Append(start);

            // For each character...
            for (int i = 0; i < str.Length; i++)
            {
                // If a unicode surrogate pair, write escaped unicode sequence.
                if (i + 1 < str.Length && char.IsHighSurrogate(str[i]) && char.IsLowSurrogate(str[i + 1]))
                {
                    formatter.Append(unicodeFormatter(str.Substring(i, 2)));
                    i++;
                    continue;
                }

                // If lone surrogate, write escaped unicode sequence.
                if (char.IsSurrogate(str[i]))
                {
                    formatter.Append(unicodeFormatter(str[i].ToString()));
                    continue;
                }

                // If an escape character, write escaped sequence.
                for (int j = 0; j < escapeCharacters.Length; j++)
                {
                    if (str[i] == escapeCharacters[j].Source)
                    {
                        formatter.Append(escapeCharacters[j].Escaped);
                        goto Continue;
                    }
                }

                // If not in the allowed character range, write escaped unicode character.
                bool disallowed = true;
                for (int j = 0; j < allowedCharacters.Length; j++)
                {
                    if (allowedCharacters[j].Has(str[i]))
                    {
                        disallowed = false;
                        break;
                    }
                }
                if (disallowed)
                {
                    formatter.Append(unicodeFormatter(str[i].ToString()));
                    continue;
                }

                // Else, write character as-is.
                formatter.Append(str[i]);

                Continue: { }
            }

            // Write closing.
            formatter.Append(end);

            // Return formatted string.
            return formatter.ToString();
        }

        public static string Parse(string str, string start, string end, EscapeCharacter[] escapeCharacters,
            AllowedCharacterRange[] allowedCharacters, UnicodeChecker unicodeChecker, UnicodeConverter unicodeParser)
        {
            if (!str.StartsWith(start) || !str.EndsWith(end))
                throw new FormatException("Input does not contain valid start/end delimiters.");

            int i = start.Length;
            int limit = str.Length - end.Length;

            StringBuilder parsed = new StringBuilder();

            while (i < limit)
            {
                // Escaped characters.
                bool matched = false;
                for (int j = 0; j < escapeCharacters.Length; j++)
                {
                    string esc = escapeCharacters[j].Escaped;
                    if (i + esc.Length <= limit &&
                        string.Compare(str, i, esc, 0, esc.Length, StringComparison.Ordinal) == 0)
                    {
                        parsed.Append(escapeCharacters[j].Source);
                        i += esc.Length;
                        matched = true;
                        break;
                    }
                }

                if (matched)
                    continue;

                // Unicode sequence.
                int unicodeLength = unicodeChecker(str, i);
                if (unicodeLength > 0)
                {
                    if (i + unicodeLength > limit)
                        throw new FormatException("Truncated unicode sequence.");

                    parsed.Append(unicodeParser(str.Substring(i, unicodeLength)));
                    i += unicodeLength;
                    continue;
                }

                // Normal character.
                parsed.Append(str[i]);
                i++;
            }

            // Return parsed string.
            return parsed.ToString();
        }
    }
}