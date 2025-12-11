using System;
using System.Text;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A utility for creating CSCD char and string literals.
    /// </summary>
    internal static class StringUtility
    {
        /* Public methods. */
        /// <summary>
        /// Takes a CSCD string and converts it to a C# string.
        /// </summary>
        public static string Parse(string str, char quote)
        {
            // Check for illegal literals.
            StringBuilder builder = new();
            int unicodeStart = -1;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                // Escaped characters.
                if (c == '\\')
                {
                    // Close unicode escape sequence.
                    if (unicodeStart >= 0)
                    {
                        string hex = str.Substring(unicodeStart + 1, i - unicodeStart - 1);
                        builder.Append(UnicodeUtility.HextoUnicode(hex));
                        unicodeStart = -1;
                    }

                    // Start escape sequence.
                    else if (i < str.Length - 1)
                    {
                        char escape = str[i + 1];
                        if (escape == '\\')
                        {
                            builder.Append('\\');
                            i++;
                        }
                        else if (escape == '\'')
                        {
                            builder.Append('\'');
                            i++;
                        }
                        else if (escape == '\"')
                        {
                            builder.Append('\"');
                            i++;
                        }
                        else if (escape == 't')
                        {
                            builder.Append('\t');
                            i++;
                        }
                        else if (escape == 'n')
                        {
                            builder.Append('\n');
                            i++;
                        }
                        else if (UnicodeUtility.IsHexCharacter(escape))
                        {
                            unicodeStart = i;
                            i++;
                        }
                        else
                            throw new Exception($"Illegal escape character '\\{escape}'.");
                    }
                    else
                        throw new Exception("Unescaped escape character.");
                }

                // Ignore characters when inside unicode escape sequence.
                else if (unicodeStart >= 0)
                    continue;

                // Normal characters: append to buffer.
                else if (CharUtility.Check(c)
                    && (c != quote || quote == '\'')
                    && !(c >= '\t' && c <= '\r')
                    && unicodeStart < 0)
                {
                    builder.Append(c);
                    continue;
                }

                // Don't allow certain characters.
                else if (c > '~' + 1)
                    throw new ArgumentException($"Illegal raw unicode character '{(long)c}'. Use '\\####\\' instead.");
                else if (c == quote && quote != '\'')
                    throw new ArgumentException($"Illegal raw quote character. Use '\\{quote}' instead.");
                else if (c == '\t')
                    throw new ArgumentException("Illegal raw tab character. Use '\\t' instead.");
                else if (c == '\n')
                    throw new ArgumentException("Illegal raw newline character. Use '\\n' instead.");
                else
                {
                    string hex = UnicodeUtility.CodePointToHex(c);
                    throw new ArgumentException($"Illegal raw control character {hex}. Use '\\{hex}\\' instead.");
                }
            }
            if (unicodeStart >= 0)
                throw new Exception("Unclosed unicode character.");

            return builder.ToString();
        }
    }
}