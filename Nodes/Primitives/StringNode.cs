using System;
using System.Text;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A string serializer node.
/// </summary>
public readonly struct StringNode : INode
{
    /* Fields. */
    private readonly string value;

    /* Public properties. */
    public readonly string Value => value;

    /* Constructors. */
    public StringNode(string value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "string: " + value;
    }

    public readonly string Serialize()
    {
        StringBuilder str = new();
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];

            // Escaped characters.
            if (c == '\\')
                str.Append("\\\\");
            else if (c == '"')
                str.Append("\\\"");
            else if (c == '\t')
                str.Append("\\t");
            else if (c == '\n')
                str.Append("\\n");
            else if (c == '\0')
                str.Append("\\0");

            // Handle unicode characters.
            else if (c < ' ' || c > '~')
                str.Append("\\[" + ((long)c).ToString("X") + "]");

            // Otherwise, append character as-is.
            else
                str.Append(value[i]);
        }

        // Enclose in double quotes.
        return '"' + str.ToString() + '"';
    }

    public static StringNode Parse(string text)
    {
        // Remove whitespaces.
        string trimmed = text?.Trim();

        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Enforce double quotes.
            if (!trimmed.StartsWith('"') || !trimmed.EndsWith('"'))
                throw new ArgumentException("Missing double-quotes.");

            // Extract contents.
            string contents = trimmed.Substring(1, trimmed.Length - 2);

            // Check for illegal literals.
            StringBuilder builder = new();
            for (int i = 0; i < contents.Length; i++)
            {
                // Normal characters: append to buffer.
                if (contents[i] >= ' ' && contents[i] <= '~' && contents[i] != '\\' && contents[i] != '"')
                {
                    builder.Append(contents[i]);
                    continue;
                }

                // Escaped characters.
                if (contents[i] == '\\' && i < contents.Length - 1)
                {
                    switch (contents[i + 1])
                    {
                        case '\\':
                            builder.Append('\\');
                            i++;
                            break;
                        case '\'':
                            builder.Append('\'');
                            i++;
                            break;
                        case '\"':
                            builder.Append('\"');
                            i++;
                            break;
                        case 't':
                            builder.Append('\t');
                            i++;
                            break;
                        case 'n':
                            builder.Append('\n');
                            i++;
                            break;
                        case '0':
                            builder.Append('\0');
                            i++;
                            break;
                        case '[':
                            for (int j = i + 2; j < contents.Length; j++)
                            {
                                if (contents[j] == ']')
                                {
                                    string unicode = contents.Substring(i + 2, j - (i + 2));
                                    builder.Append(UnicodeParser.Parse(unicode));
                                    i = j;
                                    break;
                                }
                                else if (j == contents.Length - 1)
                                    throw new Exception("Unclosed unicode escape sequence.");
                            }
                            break;
                        default:
                            throw new Exception($"Illegal escape character '\\{contents[i + 1]}'.");
                    }
                    continue;
                }

                // Don't allow certain characters.
                if (contents[i] > '~' + 1)
                    throw new ArgumentException($"Illegal raw unicode character '{(long)contents[i]}'. Use '\\[####]' instead.");
                switch (contents[i])
                {
                    case '\\':
                        throw new ArgumentException("Illegal raw backslash character. Use '\\\\' instead.");
                    case '\"':
                        throw new ArgumentException("Illegal raw double-quote character. Use '\\\"' instead.");
                    case '\t':
                        throw new ArgumentException("Illegal raw tab character. Use '\\t' instead.");
                    case '\n':
                        throw new ArgumentException("Illegal raw newline character. Use '\\n' instead.");
                    case '\0':
                        throw new ArgumentException("Illegal raw null character. Use '\\0' instead.");
                    default:
                        throw new ArgumentException($"Illegal raw control character {(long)contents[1]}. Use '\\[####]' instead.");
                }
            }

            return new StringNode(builder.ToString());
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a string:\n{ex.Message}");
        }
    }
}