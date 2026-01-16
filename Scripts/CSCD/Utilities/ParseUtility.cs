using System;
using System.Collections.Generic;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// An utility for parsing CSCD strings.
    /// </summary>
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
                return new();

            // Parse text.
            List<string> result = new();
            bool inType = false;
            bool inChar = false;
            bool inString = false;
            bool inTime = false;
            bool inComment = false;
            bool inUnicode = false;
            int depth = 0;
            int start = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (inType)
                {
                    if (c == ')')
                        inType = false;
                }

                else if (inChar)
                {
                    if (c == '\'' && text[i - 1] != '\\')
                        inChar = false;
                }

                else if (inString)
                {
                    if (c == '\\')
                    {
                        if (!inUnicode && i + 1 < text.Length)
                        {
                            if (IsHex(text[i + 1]))
                                inUnicode = true;
                            else
                                i++;
                        }
                        else if (inUnicode)
                            inUnicode = false;
                    }
                    else if (c == '"')
                        inString = false;
                }

                else if (inTime)
                {
                    if (c == ';')
                        inTime = false;
                }

                else if (inComment)
                {
                    if (c == '*' && i + 1 < text.Length && text[i + 1] == '/')
                    {
                        inComment = false;
                        i++;
                    }
                }

                else if (c == '(')
                    inType = true;

                else if (c == '\'')
                    inChar = true;

                else if (c == '"')
                    inString = true;

                else if (c == '@')
                    inTime = true;

                else if (c == '/' && i + 1 < text.Length && text[i + 1] == '*')
                {
                    inComment = true;
                    i++;
                }

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

        /* Private methods. */
        private static bool IsHex(char c)
        {
            return c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' || c <= 'z';
        }
    }
}