using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rusty.Serialization.Core.Nodes
{
    internal static class ParseUtility
    {
        /* Public methods. */
        /// <summary>
        /// Takes a delimited string and parses it into a list of strings.
        /// </summary>
        public static List<string> Split(string text, char delimiter = ',')
        {
            // Handle null.
            if (text == null)
                throw new ArgumentException("Cannot parse null.");

            // Remove trailing whitespace.
            text = text.Trim();

            // Handle empty list.
            if (text.Length == 0)
                return [];

            // Parse text.
            List<string> result = new();
            bool inChar = false;
            bool inString = false;
            bool inType = false;
            int depth = 0;
            int start = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (inString)
                {
                    if (!inChar && c == '"')
                    {
                        bool isDoubled = i + 1 < text.Length && text[i + 1] == '"';
                        if (isDoubled)
                            i++;
                        else
                            inString = false;
                    }
                }

                else if (inChar && c == '\'')
                    inChar = false;

                else if (!inChar && c == '"')
                    inString = true;

                else if (c == '\'')
                    inChar = true;

                else if (!inType && c == '(')
                    inType = true;

                else if (inType && c == ')')
                    inType = false;

                else if (c == delimiter && depth == 0)
                {
                    string segment = text.Substring(start, i - start).Trim();
                    result.Add(segment);
                    start = i + 1;
                }

                else
                {
                    switch (c)
                    {
                        case '[':
                        case '{':
                        case '<':
                            depth++;
                            break;
                        case ']':
                        case '}':
                        case '>':
                            depth--;
                            break;
                    }
                }
            }

            string final = text.Substring(start).Trim();
            result.Add(final);

            return result;
        }

        /// <summary>
        /// Parse a trimmed string and return a serializer node of the appropriate type.
        /// </summary>
        public static INode ParseValue(string text)
        {
            if (text == null)
                throw new ArgumentException("Cannot parse null.");
            if (text.StartsWith('#'))
                return ColorNode.Parse(text);
            if (text.StartsWith('\'') && text.EndsWith('\''))
                return CharNode.Parse(text);
            if (text.StartsWith('"') && text.EndsWith('"'))
                return StringNode.Parse(text);
            if (text.StartsWith('('))
                return TypeNode.Parse(text);
            if (text.StartsWith('[') && text.EndsWith(']'))
                return ListNode.Deserialize(text);
            if (text.StartsWith('{') && text.EndsWith('}'))
                return DictNode.Deserialize(text);
            if (text.StartsWith('<') && text.EndsWith('>'))
                return ObjectNode.Deserialize(text);
            if (text.ToLower() == "true" || text.ToLower() == "false")
                return BoolNode.Parse(text);
            if (text.ToLower() == "null")
                return NullNode.Parse(text);
            if (text.StartsWith('Y') || text.StartsWith("-Y")
                || text.StartsWith('M') || text.StartsWith("-M")
                || text.StartsWith('D') || text.StartsWith("-D")
                || text.StartsWith('h') || text.StartsWith("-h")
                || text.StartsWith('m') || text.StartsWith("-m")
                || text.StartsWith('s') || text.StartsWith("-s")
                || text.StartsWith('f') || text.StartsWith("-f"))
            {
                return TimeNode.Parse(text);
            }
            if (text.Contains('.'))
                return RealNode.Parse(text);
            if (decimal.TryParse(text, CultureInfo.InvariantCulture, out decimal d))
                return IntNode.Parse(text);
            throw new ArgumentException($"Invalid string '{text}'");
        }
    }
}