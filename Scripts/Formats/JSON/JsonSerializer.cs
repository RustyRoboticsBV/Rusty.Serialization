using System;
using System.Text;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON serializer for node trees.
    /// </summary>
    public class JsonSerializer : Serializer
    {
        /* Public methods. */
        public override string Serialize(NodeTree node, Settings settings)
        {
            return Serialize(node.Root, settings);
        }

        /* Private methods. */
        private static string Serialize(INode node, Settings settings)
        {
            StringBuilder sb = new StringBuilder();
            bool prettyPrint = settings.PrettyPrint;

            string idStr = null;
            if (node is IdNode id)
            {
                idStr = id.Name;
                node = id.Value;
            }

            string typeStr = null;
            if (node is TypeNode type)
            {
                typeStr = type.Name;
                node = type.Value;
            }

            switch (node)
            {
                case NullNode:
                    AddName(sb, "$null");
                    AddColon(sb, prettyPrint);
                    sb.Append("null");
                    break;

                case BoolNode @bool:
                    AddName(sb, "$bool");
                    AddColon(sb, prettyPrint);
                    sb.Append(@bool.Value ? "true" : "false");
                    break;

                case IntNode @int:
                    AddName(sb, "$int");
                    AddColon(sb, prettyPrint);
                    sb.Append(@int.Value.ToString());
                    break;

                case FloatNode @float:
                    AddName(sb, "$float");
                    AddColon(sb, prettyPrint);
                    sb.Append(@float.Value.ToString());
                    break;

                case InfinityNode inf:
                    AddName(sb, "$inf");
                    AddColon(sb, prettyPrint);
                    if (inf.Positive)
                        AddName(sb, "+");
                    else
                        AddName(sb, "-");
                    break;

                case NanNode nan:
                    AddName(sb, "$nan");
                    AddColon(sb, prettyPrint);
                    AddName(sb, "nan");
                    break;

                case CharNode @char:
                    AddName(sb, "$char");
                    AddColon(sb, prettyPrint);
                    AddName(sb, @char.Value.ToString());
                    break;

                case StringNode str:
                    AddName(sb, "$str");
                    AddColon(sb, prettyPrint);
                    AddName(sb, str.Value);
                    break;

                case DecimalNode dec:
                    AddName(sb, "$dec");
                    AddColon(sb, prettyPrint);
                    sb.Append(dec.Value.ToString());
                    break;

                case ColorNode color:
                    AddName(sb, "$col");
                    AddColon(sb, prettyPrint);
                    AddName(sb, color.Value.ToString());
                    break;

                case TimestampNode time:
                    AddName(sb, "$time");
                    AddColon(sb, prettyPrint);
                    AddName(sb, time.Value.ToString());
                    break;

                case BytesNode bytes:
                    AddName(sb, "$bytes");
                    AddColon(sb, prettyPrint);
                    AddName(sb, bytes.Value.ToString());
                    break;

                case SymbolNode symbol:
                    AddName(sb, "$symbol");
                    AddColon(sb, prettyPrint);
                    AddName(sb, symbol.Name);
                    break;

                case ListNode list:
                    AddName(sb, "$list");
                    AddColon(sb, prettyPrint);

                    StringBuilder items = new StringBuilder();
                    for (int i = 0; i < list.Count; i++)
                    {
                        items.Append(Serialize(list.GetValueAt(i), settings));

                        if (i < list.Count - 1)
                            AddComma(items, prettyPrint);
                    }

                    Wrap(items, "[", "]", prettyPrint);
                    sb.Append(items.ToString());
                    break;

                case DictNode dict:
                    AddName(sb, "$dict");
                    AddColon(sb, prettyPrint);

                    StringBuilder entries = new StringBuilder();

                    for (int i = 0; i < dict.Count; i++)
                    {
                        StringBuilder pair = new StringBuilder();

                        pair.Append(Serialize(dict.Pairs[i].Key, settings));
                        AddComma(pair, prettyPrint);

                        pair.Append(Serialize(dict.Pairs[i].Value, settings));

                        Wrap(pair, "[", "]", prettyPrint);
                        entries.Append(pair.ToString());

                        if (i < dict.Count - 1)
                            AddComma(entries, prettyPrint);
                    }

                    Wrap(entries, "[", "]", prettyPrint);
                    sb.Append(entries.ToString());
                    break;

                case ObjectNode obj:
                    AddName(sb, "$obj");
                    AddColon(sb, prettyPrint);

                    StringBuilder members = new StringBuilder();
                    for (int i = 0; i < obj.Count; i++)
                    {
                        AddName(members, obj.Members[i].Key.ToString()); // TODO: fix.
                        AddColon(members, prettyPrint);
                        members.Append(Serialize(obj.Members[i].Value, settings));

                        if (i < obj.Count - 1)
                            AddComma(members, prettyPrint);
                    }
                    Wrap(members, "{", "}", prettyPrint);
                    sb.Append(members.ToString());

                    break;

                default:
                    throw new ArgumentException($"Invalid node type '{node.GetType()}'.");
            }

            Wrap(sb, "{", "}", prettyPrint, idStr, typeStr);
            return sb.ToString();
        }

        private static void AddName(StringBuilder sb, string name)
        {
            sb.Append('"');
            sb.Append(name);
            sb.Append('"');
        }

        private static void AddColon(StringBuilder sb, bool prettyPrint)
        {
            if (prettyPrint)
                sb.Append(' ');
            sb.Append(':');
            if (prettyPrint)
                sb.Append(' ');
        }

        private static void AddComma(StringBuilder sb, bool prettyPrint)
        {
            sb.Append(',');
            if (prettyPrint)
                sb.Append('\n');
        }

        private static void Wrap(StringBuilder sb, string start, string end, bool prettyPrint, string idStr = null, string typeStr = null)
        {
            string str = sb.ToString();

            sb.Clear();

            sb.Append(start);
            if (prettyPrint)
                sb.Append('\n');

            if (idStr != null)
            {
                if (prettyPrint)
                    sb.Append("  ");
                AddName(sb, "$id");
                AddColon(sb, prettyPrint);
                AddName(sb, idStr);
                AddComma(sb, prettyPrint);
            }

            if (typeStr != null)
            {
                if (prettyPrint)
                    sb.Append("  ");
                AddName(sb, "$type");
                AddColon(sb, prettyPrint);
                AddName(sb, typeStr);
                AddComma(sb, prettyPrint);
            }

            if (prettyPrint)
                str = "  " + str.Replace("\n", "\n  ");
            sb.Append(str);

            if (prettyPrint)
                sb.Append('\n');
            sb.Append(end);
        }
    }
}