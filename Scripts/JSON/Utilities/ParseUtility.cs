using System;
using System.Collections.Generic;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// An utility for parsing JSON strings.
    /// </summary>
    internal static class ParseUtility
    {
        /* Public methods. */
        /// <summary>
        /// Takes a delimited string and parses it into a list of strings.
        /// </summary>
        public static List<string> Split(string text, char quote = ',')
        {
            // Handle null.
            if (text == null)
                throw new ArgumentException("Cannot parse null.");

            // Remove trailing whitespace.
            text = text.Trim();

            // Handle empty list.
            if (text.Length == 0)
                return new();

            // Parse text.
            List<string> result = new();
            bool inChar = false;
            bool inString = false;
            bool inType = false;
            bool inComment = false;
            int depth = 0;
            int start = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (inComment)
                {
                    if (c == '*' && i + 1 < text.Length && text[i + 1] == '/')
                    {
                        inComment = false;
                        i++;
                    }
                }

                else if (inString)
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

                else if (c == '/' && i + 1 < text.Length && text[i + 1] == '*')
                {
                    inComment = true;
                    i++;
                }

                else if (c == quote && depth == 0)
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
    }
}