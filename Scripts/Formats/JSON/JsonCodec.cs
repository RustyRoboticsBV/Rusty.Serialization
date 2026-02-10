using System;
using System.Text;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON serializer/deserializer back-end.
    /// </summary>
    public class JsonCodec : Codec
    {
        /* Public methods. */
        public override string Serialize(NodeTree node, Settings settings)
        {
            return Serialize(node.Root, settings);
        }

        public override NodeTree Parse(string serialized)
        {
            throw new NotImplementedException();
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
                    AddName(sb, "$const");
                    AddColon(sb, prettyPrint);
                    sb.Append("null");
                    break;

                case BoolNode @bool:
                    AddName(sb, "$const");
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
                    AddName(sb, "$const");
                    AddColon(sb, prettyPrint);
                    if (inf.Positive)
                        AddName(sb, "inf");
                    else
                        AddName(sb, "-inf");
                    break;

                case NanNode nan:
                    AddName(sb, "$const");
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
                    AddName(sb, dec.Value.ToString());
                    break;

                case ColorNode color:
                    AddName(sb, "$col");
                    AddColon(sb, prettyPrint);
                    AddName(sb, color.Value.ToString());
                    break;

                case TimeNode time:
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

                case DictNode dict:
                    AddName(sb, "$dict");
                    AddColon(sb, prettyPrint);

                    sb.Append('{');
                    for (int i = 0; i < dict.Count; i++)
                    {
                    }
                    sb.Append('}');

                    break;

                case ObjectNode obj:
                    AddName(sb, "$obj");
                    AddColon(sb, prettyPrint);

                    sb.Append('{');
                    for (int i = 0; i < obj.Count; i++)
                    {
                    }
                    sb.Append('}');

                    break;

                default:
                    throw new ArgumentException($"Invalid node type '{node.GetType()}'.");
            }

            Wrap(sb, "{", "}", prettyPrint);
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

        private static void Wrap(StringBuilder sb, string start, string end, bool prettyPrint)
        {
            string str = sb.ToString();

            sb.Clear();

            sb.Append(str);
            if (prettyPrint)
                sb.Append('\n');

            if (prettyPrint)
                str.Replace("\n", "\n  ");
            sb.Append(str);

            if (prettyPrint)
                sb.Append('\n');
            sb.Append(str);
        }
    }
}