using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// A character serializer node.
/// </summary>
public readonly struct CharNode : INode
{
    /* Fields. */
    private readonly char value;

    /* Public properties. */
    public readonly char Value => value;

    /* Constructors. */
    public CharNode(char value)
    {
        this.value = value;
    }

    /* Public methods. */
    public override readonly string ToString()
    {
        return "char: " + value;
    }

    public readonly string Serialize()
    {

        return $"'{value.ToString(CultureInfo.InvariantCulture)}'";
    }

    public static CharNode Parse(string text)
    {
        string trimmed = text?.Trim();
        try
        {
            // Empty strings are not allowed.
            if (string.IsNullOrEmpty(trimmed))
                throw new ArgumentException("Empty string.");

            // Enforce quotes.
            if (!(trimmed.StartsWith('\'') && trimmed.EndsWith('\'')))
                throw new ArgumentException("Missing quotes.");

            // Empty characters not allowed.
            if (trimmed.Length == 2)
                throw new ArgumentException("Empty character.");

            // Normal character.
            if (trimmed.Length == 3)
            {
                if (trimmed[1] >= ' ' && trimmed[1] <= '~')
                    return new(trimmed[1]);
                else
                {
                    if (trimmed[1] > '~' + 1)
                        throw new ArgumentException($"Illegal raw unicode character '{(long)trimmed[1]}'. Use '\\[####]' instead.");
                    switch (trimmed[1])
                    {
                        case '\t':
                            throw new ArgumentException("Illegal raw tab character. Use '\\t' instead.");
                        case '\n':
                            throw new ArgumentException("Illegal raw newline character. Use '\\n' instead.");
                        case '\0':
                            throw new ArgumentException("Illegal raw null character. Use '\\0' instead.");
                        default:
                            throw new ArgumentException($"Illegal raw control character {(long)trimmed[1]}. Use '\\[####]' instead.");
                    }
                }
            }

            // Escaped characters.
            else if (trimmed.Length == 4 && trimmed.StartsWith("'\\") && trimmed.EndsWith("'"))
            {
                switch (trimmed[2])
                {
                    case '\\':
                        return new('\\');
                    case '\'':
                        return new('\'');
                    case '\"':
                        return new('\"');
                    case 't':
                        return new('\t');
                    case 'n':
                        return new('\n');
                    case '0':
                        return new('\0');
                    default:
                        throw new ArgumentException($"Illegal escape character '\\{trimmed[2]}.'");
                }
            }

            // Unicode.
            else if (trimmed.StartsWith("'\\[") && trimmed.EndsWith("]'"))
            {
                string unicode = trimmed.Substring(3, trimmed.Length - 5);
                if (unicode == "")
                    throw new ArgumentException("Empty unicode.");
                else
                    return new(UnicodeParser.Parse(unicode));
            }

            throw new ArgumentException("Too many characters.");
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not parse string '{text}' as a character:\n{ex.Message}");
        }
    }
}