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
        public static List<string> Split(string text, char separator = ',')
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
            bool inString = false;
            int depth = 0;
            int start = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (inString && i + 1 < text.Length && text[i] == '\\' && text[i + 1] == '"')
                {
                    result.Add("\"");
                    i++;
                }

                else if (c == '"')
                    inString = !inString;

                else if (c == separator && depth == 0)
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
                            depth++;
                            break;
                        case ']':
                        case '}':
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