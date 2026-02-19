using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD serializer.
    /// </summary>
    public class CscdSerializer : Serializer
    {
        /* Private constants. */
        private readonly static HashSet<UnicodePair> idEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '`', '\\' };
        private readonly static HashSet<UnicodePair> typeEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', ')', '\\' };
        private readonly static HashSet<UnicodePair> scopeEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '^', '\\' };
        private readonly static HashSet<UnicodePair> charEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r' };
        private readonly static HashSet<UnicodePair> strEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '"', '\\' };
        private readonly static HashSet<UnicodePair> symbolEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '*', '\\' };
        private readonly static HashSet<UnicodePair> refEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', '&', '\\' };

        private static readonly Dictionary<UnicodePair, char> simpleEscapes = new Dictionary<UnicodePair, char>()
        {
            { '\t', 't' },
            { '\n', 'n' },
            { '\r', 'r' },
            { '"', '"' },
            { '&', '&' },
            { '\'', '\'' },
            { '(', '(' },
            { ')', ')' },
            { '*', '*' },
            { '\\', '\\' },
            { '^', '^' },
            { '`', '`' }
        };

        private static string tab = "  ";

        /* Public methods. */
        public override string Serialize(NodeTree tree, Settings settings)
        {
            string str = Serialize(tree.Root, settings.PrettyPrint);

            if (settings.IncludeFormatHeader)
            {
                str = "~CSCD~" + (settings.PrettyPrint ? "\n" : "")
                    + str
                    + (settings.PrettyPrint ? "\n" : "") + "~/CSCD~";
            }

            return str;
        }

        /* Private methods. */
        private string Serialize(INode node, bool prettyPrinting)
        {
            if (node is IdNode id)
                return $"`{FormatText(id.Name, idEscapes)}`{Serialize(id.Value, prettyPrinting)}";
            if (node is TypeNode type)
                return $"({FormatText(type.Name, typeEscapes)}){Serialize(type.Value, prettyPrinting)}";
            if (node is ScopeNode scope)
                return $"^{FormatText(scope.Name, scopeEscapes)}^{Serialize(scope.Value, prettyPrinting)}";
            if (node is NullNode)
                return "null";
            if (node is BoolNode b)
                return b.Value ? "true" : "false";
            if (node is IntNode i)
                return i.Value.ToString();
            if (node is FloatNode f)
                return Serialize(f);
            if (node is InfinityNode inf)
                return inf.Positive ? "inf" : "-inf";
            if (node is NanNode)
                return "nan";
            if (node is CharNode chr)
                return Serialize(chr);
            if (node is StringNode str)
                return $"\"{FormatText(str.Value, strEscapes)}\"";
            if (node is DecimalNode dec)
                return Serialize(dec);
            if (node is ColorNode col)
                return Serialize(col);
            if (node is TimestampNode t)
                return Serialize(t);
            if (node is DurationNode d)
                return d.Value.ToString();
            if (node is BytesNode byt)
                return Serialize(byt);
            if (node is SymbolNode sb)
                return Serialize(sb, false);
            if (node is RefNode rf)
                return $"&{FormatText(rf.ID, refEscapes)}&";
            if (node is ListNode lst)
                return Serialize(lst, prettyPrinting);
            if (node is DictNode dic)
                return Serialize(dic, prettyPrinting);
            if (node is ObjectNode obj)
                return Serialize(obj, prettyPrinting);

            throw new ArgumentException($"Unexpected node of type {node.GetType()}.");
        }

        private string Serialize(FloatNode node)
        {
            string value = node.Value.ToString();

            bool negative = value.StartsWith("-");
            if (negative)
                value = value.Substring(1);

            int dotIndex = value.IndexOf('.');
            if (dotIndex == -1)
            {
                dotIndex = value.Length;
                value = value + '.';
            }

            string integer = value.Substring(0, dotIndex);
            string fractional = value.Substring(dotIndex + 1);

            integer = integer.TrimStart('0');

            fractional = fractional.TrimEnd('0');

            string result = integer + "." + fractional;

            if (negative)
                result = "-" + result;

            return result;
        }

        private string Serialize(CharNode node)
        {
            if (node.Value == '\0')
                return "''";
            return $"'{FormatText(node.Value.ToString(), charEscapes)}'";
        }

        private string Serialize(DecimalNode node)
        {
            return node.Value.negative ? $"-${node.Value.ToString().Substring(1)}" : $"${node.Value}";
        }

        private string Serialize(ColorNode node)
        {
            // Handle clear.
            if (node.Value.r == 0 && node.Value.g == 0 && node.Value.b == 0 && node.Value.a == 0)
                return "#";

            // Format color channels.
            Span<char> span = stackalloc char[8];
            FormatColorChannel(node.Value.r, span.Slice(0, 2));
            FormatColorChannel(node.Value.g, span.Slice(2, 2));
            FormatColorChannel(node.Value.b, span.Slice(4, 2));
            FormatColorChannel(node.Value.a, span.Slice(6, 2));

            // Figure out format & length.
            bool shortForm = span[0] == span[1]
                && span[2] == span[3]
                && span[4] == span[5]
                && span[6] == span[7];

            bool hasAlpha = span[6] != 'F' || span[7] != 'F';

            int length = shortForm ? (hasAlpha ? 5 : 4) : (hasAlpha ? 9 : 7);

            // Create string.
            Span<char> result = stackalloc char[length];

            result[0] = '#';
            if (shortForm)
            {
                result[1] = span[0];
                result[2] = span[2];
                result[3] = span[4];
                if (hasAlpha)
                    result[4] = span[6];
            }
            else
                span.Slice(0, hasAlpha ? 8 : 6).CopyTo(result.Slice(1));

            return new string(result);
        }

        private string Serialize(TimestampNode node)
        {
            bool noDate = node.Value.year == 1 && node.Value.month == 1 && node.Value.day == 1;
            bool noTime = node.Value.hour == 0 && node.Value.minute == 0 && node.Value.second == 0;

            if (noDate && noTime)
                return "@@";
            else if (noTime)
                return $"@{node.Value.year}/{node.Value.month}/{node.Value.day}@";
            else if (noDate)
                return $"@{node.Value.hour}:{node.Value.minute}:{node.Value.second}@";
            else
                return $"@{node.Value.year}/{node.Value.month}/{node.Value.day},{node.Value.hour}:{node.Value.minute}:{node.Value.second}@";
        }

        private string Serialize(BytesNode node)
        {
            ReadOnlySpan<char> src = node.Value.AsSpan();

            // Trim padding.
            int end = src.Length;
            while (end > 0 && src[end - 1] == '=')
            {
                end--;
            }
            src = src.Slice(0, end);

            // Add ! prefix.
            Span<char> buffer = stackalloc char[src.Length + 1];
            buffer[0] = '!';
            src.CopyTo(buffer.Slice(1));
            return new string(buffer);
        }

        private string Serialize(SymbolNode node, bool allowReservedKeywords)
        {
            // Check if the symbol can be bare or not.
            bool canBeBare = false;
            if (node.Name.Length > 0)
            {
                char c = node.Name[0];
                canBeBare = c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_';
            }
            for (int i = 1; i < node.Name.Length; i++)
            {
                char c = node.Name[i];
                if (!(c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9' || c == '_'))
                {
                    canBeBare = false;
                    break;
                }
            }
            if (!allowReservedKeywords
                && (node.Name == "null" || node.Name == "false" || node.Name == "true" || node.Name == "nan" || node.Name == "inf"))
            {
                canBeBare = false;
            }

            // Create symbol text.
            if (canBeBare)
                return node.Name;
            else
                return $"*{FormatText(node.Name, symbolEscapes)}*";
        }

        private string Serialize(ListNode node, bool prettyPrinting)
        {
            if (node.Count == 0)
                return "[]";

            StringBuilder sb = StringBuilders.Rent();
            sb.Append('[');
            for (int i = 0; i < node.Count; i++)
            {
                // Comma.
                if (i > 0)
                    sb.Append(',');
                if (prettyPrinting)
                {
                    sb.Append("\n");
                    sb.Append(tab);
                }

                // Element.
                string element = Serialize(node.GetValueAt(i), prettyPrinting);
                if (prettyPrinting)
                    element = element.Replace("\n", "\n" + tab);
                sb.Append(element);
            }
            if (prettyPrinting)
                sb.Append('\n');
            sb.Append(']');
            return sb.ToString();
        }

        private string Serialize(DictNode node, bool prettyPrinting)
        {
            if (node.Count == 0)
                return "{}";

            StringBuilder sb = StringBuilders.Rent();
            sb.Append('{');
            for (int i = 0; i < node.Count; i++)
            {
                // Comma.
                if (i > 0)
                    sb.Append(',');
                if (prettyPrinting)
                {
                    sb.Append("\n");
                    sb.Append(tab);
                }

                // Key.
                string key = Serialize(node.GetKeyAt(i), prettyPrinting);
                if (prettyPrinting)
                    key = key.Replace("\n", "\n" + tab);
                sb.Append(key);

                // Colon.
                if (prettyPrinting)
                    sb.Append(" : ");
                else
                    sb.Append(':');

                // Value.
                string value = Serialize(node.GetValueAt(i), prettyPrinting);
                if (prettyPrinting)
                    value = value.Replace("\n", "\n" + tab);
                sb.Append(value);
            }
            if (prettyPrinting)
                sb.Append('\n');
            sb.Append('}');
            return sb.ToString();
        }

        private string Serialize(ObjectNode node, bool prettyPrinting)
        {
            if (node.Count == 0)
                return "<>";

            StringBuilder sb = StringBuilders.Rent();
            sb.Append('<');
            for (int i = 0; i < node.Count; i++)
            {
                // Comma.
                if (i > 0)
                    sb.Append(',');
                if (prettyPrinting)
                {
                    sb.Append("\n");
                    sb.Append(tab);
                }

                // Name.
                sb.Append(Serialize(node.GetNameAt(i), true));

                // Colon.
                if (prettyPrinting)
                    sb.Append(" : ");
                else
                    sb.Append(':');

                // Value.
                string value = Serialize(node.GetValueAt(i), prettyPrinting);
                if (prettyPrinting)
                    value = value.Replace("\n", "\n" + tab);
                sb.Append(value);
            }
            if (prettyPrinting)
                sb.Append('\n');
            sb.Append('>');
            return sb.ToString();
        }

        // Helper methods.

        private static string FormatText(string text, HashSet<UnicodePair> escapeSequences)
        {
            StringBuilder sb = StringBuilders.Rent(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                // Escape illegal characters.
                if (!(c == 0x09 || c == 0x0A || c == 0x0D || c == 0x20
                    || c >= 0x21 && c <= 0x7E
                    || c >= 0xA1 && c <= 0xAC
                    || c >= 0xAE && c <= 0xFF) || escapeSequences.Contains(c))
                {
                    FormatEscaped(c, sb);
                }

                // Append character as-is.
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private static void FormatEscaped(UnicodePair chr, StringBuilder sb)
        {
            sb.Append('\\');
            if (simpleEscapes.TryGetValue(chr, out char escaped))
                sb.Append(escaped);
            else
            {
                sb.Append(chr.Hex);
                sb.Append(';');
            }
        }

        private static void FormatColorChannel(byte col, Span<char> span)
        {
            span[0] = ToHex(col >> 4);
            span[1] = ToHex(col & 0xF);
        }

        private static char ToHex(int value)
        {
            if (value > 15)
                throw new ArgumentOutOfRangeException(nameof(value));
            return (char)(value < 10 ? '0' + value : 'A' + (value - 10));
        }
    }
}