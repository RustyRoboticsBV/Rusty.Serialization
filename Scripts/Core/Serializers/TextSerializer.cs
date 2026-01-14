using Rusty.Serialization.Core.Nodes;
using System;
using System.Text;

namespace Rusty.Serialization.Core.Serializers
{
    /// <summary>
    /// A text node serializer. Should be used for CharNode and StringNode.
    /// </summary>
    public abstract class TextSerializer<NodeT> : Serializer<NodeT>
        where NodeT : ITextNode, new()
    {
        /* Public types. */
        public readonly struct EscapeCharacter
        {
            public readonly char Source { get; }
            public readonly string Escaped { get; }

            public EscapeCharacter(char source, string escaped)
            {
                Source = source;
                Escaped = escaped;
            }

            public static implicit operator EscapeCharacter((char, string) range)
            {
                return new EscapeCharacter(range.Item1, range.Item2);
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

            public static implicit operator AllowedCharacterRange((int, int) range)
            {
                return new AllowedCharacterRange(range.Item1, range.Item2);
            }

            public bool Has(int chr)
            {
                return chr >= Start && chr <= End;
            }
        }

        /* Protected properties. */
        protected abstract string StartDelimiter { get; }
        protected abstract string EndDelimiter { get; }
        protected abstract EscapeCharacter[] EscapeCharacters { get; }
        protected abstract AllowedCharacterRange[] AllowedCharacters { get; }

        /* Public methods. */
        public sealed override string Serialize(NodeT node, ISerializerScheme scheme)
        {
            string str = node.Value;

            StringBuilder formatter = new StringBuilder();

            // Write opening.
            formatter.Append(StartDelimiter);

            // For each character...
            for (int i = 0; i < str.Length; i++)
            {
                // If a unicode surrogate pair, write escaped unicode sequence.
                if (i + 1 < str.Length && char.IsHighSurrogate(str[i]) && char.IsLowSurrogate(str[i + 1]))
                {
                    formatter.Append(EscapeUnicode(str, i));
                    i++;
                    continue;
                }

                // If an escape character, write escaped sequence.
                for (int j = 0; j < EscapeCharacters.Length; j++)
                {
                    if (str[i] == EscapeCharacters[j].Source)
                    {
                        formatter.Append(EscapeCharacters[j].Escaped);
                        goto Continue;
                    }
                }

                // If not in the allowed character range, write escaped unicode character.
                if (!IsAllowed(str[i]))
                {
                    formatter.Append(EscapeUnicode(str, i));
                    continue;
                }

                // Else, write character as-is.
                formatter.Append(str[i]);

                Continue: { }
            }

            // Write closing.
            formatter.Append(EndDelimiter);

            // Return formatted string.
            return formatter.ToString();
        }

        public sealed override NodeT Parse(string str, ISerializerScheme scheme)
        {
            if (!str.StartsWith(StartDelimiter) || !str.EndsWith(EndDelimiter))
                throw new FormatException("Input does not contain valid start/end delimiters.");

            int i = StartDelimiter.Length;
            int limit = str.Length - EndDelimiter.Length;

            StringBuilder parsed = new StringBuilder();

            while (i < limit)
            {
                // Escaped characters.
                bool matched = false;
                for (int j = 0; j < EscapeCharacters.Length; j++)
                {
                    string esc = EscapeCharacters[j].Escaped;
                    if (i + esc.Length <= limit && string.Compare(str, i, esc, 0, esc.Length, StringComparison.Ordinal) == 0)
                    {
                        parsed.Append(EscapeCharacters[j].Source);
                        i += esc.Length;
                        matched = true;
                        break;
                    }
                }

                if (matched)
                    continue;

                // Unicode sequence.
                int unicodeLength = GetUnicodeLength(str, i);
                if (unicodeLength > 0)
                {
                    if (i + unicodeLength > limit)
                        throw new FormatException("Truncated unicode sequence.");

                    parsed.Append(ParseUnicode(str, i, unicodeLength));
                    i += unicodeLength;
                    continue;
                }

                // Disallowed character?
                if (!IsAllowed(str[i]))
                    throw new ArgumentException($"Disallowed character '{str[i]}'.");

                // Normal character.
                parsed.Append(str[i]);
                i++;
            }

            // Return node.
            NodeT node = new NodeT();
            node.Value = parsed.ToString();
            return node;
        }

        /* Protected methods. */
        protected abstract string EscapeUnicode(string text, int index);
        protected abstract int GetUnicodeLength(string text, int index);
        protected abstract UnicodePair ParseUnicode(string text, int index, int length);

        /* Private methods. */
        /// <summary>
        /// Check if a character is in the allowed range(s).
        /// </summary>
        private bool IsAllowed(int chr)
        {
            for (int j = 0; j < AllowedCharacters.Length; j++)
            {
                if (AllowedCharacters[j].Has(chr))
                    return true;
            }
            return false;
        }
    }
}