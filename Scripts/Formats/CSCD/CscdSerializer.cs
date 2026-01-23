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
        private readonly static HashSet<UnicodePair> idEscapes = new HashSet<UnicodePair> { '\t', '\n', '\r', '`', '\\' };
        private readonly static HashSet<UnicodePair> typeEscapes = new HashSet<UnicodePair> { '\t', '\n', '\r', ')', '\\' };
        private readonly static HashSet<UnicodePair> charEscapes = new HashSet<UnicodePair> { '\t', '\n', '\r' };
        private readonly static HashSet<UnicodePair> strEscapes = new HashSet<UnicodePair> { '\t', '\n', '\r', '"', '\\' };
        private readonly static HashSet<UnicodePair> refEscapes = new HashSet<UnicodePair> { '\t', '\n', '\r', ';', '\\' };
        private readonly static HashSet<UnicodePair> memberNameEscapes =
            new HashSet<UnicodePair> { '\t', '\n', '\r', ' ', ',', '/', ':', '>', '\\', ']', '}' };

        private static readonly Dictionary<UnicodePair, char> simpleEscapes = new()
        {
            { 't', '\t' },
            { 'n', '\n' },
            { 's', ' ' },
            { '"', '"' },
            { '\'', '\'' },
            { '`', '`' },
            { '(', '(' },
            { ')', ')' },
            { ';', ';' },
            { '\\', '\\' }
        };

        private static string tab = "  ";

        /* Public methods. */
        public override string Serialize(NodeTree tree, bool prettyPrint)
        {
            return Serialize(tree.Root, prettyPrint);
        }

        /* Private methods. */
        private string Serialize(INode node, bool prettyPrinting)
        {
            if (node is IdNode id)
                return $"`{FormatText(id.Name, idEscapes)}`{Serialize(id.Value, prettyPrinting)}";
            if (node is TypeNode type)
                return $"({FormatText(type.Name, typeEscapes)}){Serialize(type.Value, prettyPrinting)}";
            if (node is NullNode)
                return "null";
            if (node is BoolNode b)
                return b.Value ? "true" : "false";
            if (node is IntNode i)
                return i.Value;
            if (node is FloatNode f)
                return Serialize(f);
            if (node is InfinityNode inf)
                return inf.Positive ? "inf" : "-inf";
            if (node is NanNode)
                return "nan";
            if (node is CharNode chr)
                return $"'{FormatText(chr.Value, charEscapes)}'";
            if (node is StringNode str)
                return $"\"{FormatText(str.Value, strEscapes)}\"";
            if (node is DecimalNode dec)
                return dec.Value.IsNegative ? $"-${dec.Value}" : $"${dec.Value}";
            if (node is ColorNode col)
                return '#' + col.Value;
            if (node is TimeNode t)
                return $"@{t.Year}-{t.Month}-{t.Day}_{t.Hour}:{t.Minute}:{t.Second};";
            if (node is BytesNode byt)
                return $"b_{byt.Value}";
            if (node is RefNode rf)
                return $"&{FormatText(rf.ID, refEscapes)};";
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
            string value = node.Value;

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
                sb.Append(FormatText(node.GetNameAt(i), memberNameEscapes));

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
                    || c >= 0xAE && c <= 0xFF))
                {
                    FormatEscaped(c, escapeSequences, sb);
                }

                // Append character as-is.
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private static void FormatEscaped(UnicodePair chr, HashSet<UnicodePair> escapeSequences, StringBuilder sb)
        {
            sb.Append('\\');
            if (escapeSequences.Contains(chr))
                sb.Append(simpleEscapes[chr]);
            else
            {
                sb.Append(chr.Hex);
                sb.Append(';');
            }
        }
    }
}