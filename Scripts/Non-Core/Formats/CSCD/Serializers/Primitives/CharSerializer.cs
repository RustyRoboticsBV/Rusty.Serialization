using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD char serializer.
    /// </summary>
    public class CharSerializer : Serializer<CharNode>
    {
        /* Public methods. */
        public override string Serialize(CharNode node, ISerializerScheme scheme)
        {
            string str = "";
            if (node.Value == '\t')
                str = "\\t";
            else if (node.Value == '\n')
                str = "\\n";
            else if (node.Value == '\0')
                str = "\\0";
            else if (!CharUtility.Check(node.Value))
                str = "\\[" + UnicodeUtility.Serialize(node.Value) + "]";
            else
                str = node.Value.ToString(CultureInfo.InvariantCulture);
            return $"'" + str + "'";
        }

        public override CharNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
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
                    char c = trimmed[1];
                    if (CharUtility.Check(c) && !(c >= '\t' && c <= '\r'))
                        return new(c);
                    else
                    {
                        if (c > '~' + 1)
                            throw new ArgumentException($"Illegal raw unicode character '{(long)c}'. Use '\\[####]' instead.");
                        switch (c)
                        {
                            case '\t':
                                throw new ArgumentException("Illegal raw tab character. Use '\\t' instead.");
                            case '\n':
                                throw new ArgumentException("Illegal raw newline character. Use '\\n' instead.");
                            case '\0':
                                throw new ArgumentException("Illegal raw null character. Use '\\0' instead.");
                            default:
                                throw new ArgumentException($"Illegal raw control character {(long)c}. Use '\\[####]' instead.");
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
                        return new(UnicodeUtility.Parse(unicode));
                }

                throw new ArgumentException("Too many characters.");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a character:\n{ex.Message}");
            }
        }
    }
}