using System;
using System.Text;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSV
{
    /// <summary>
    /// A CSV serializer.
    /// </summary>
    public class CsvSerializer : Serializer
    {
        /* Public methods. */
        public override string Serialize(NodeTree tree, Settings settings)
        {
            StringBuilder sb = new StringBuilder();
            Serialize(tree.Root, sb);
            return sb.ToString();
        }

        /* Private methods. */
        private void Serialize(INode node, StringBuilder sb)
        {
            if (node is IdNode id)
            {
                sb.Append("ID,");
                sb.Append(Pack(id.Name));
                sb.Append("\n");
                Serialize(id.Value, sb);
                sb.Append("\nEND");
            }
            else if (node is TypeNode type)
            {
                sb.Append("TYPE,");
                sb.Append(Pack(type.Name));
                sb.Append("\n");
                Serialize(type.Value, sb);
                sb.Append("\nEND");
            }
            else if (node is NullNode)
                sb.Append("NULL");
            else if (node is BoolNode b)
            {
                sb.Append("BOOL,");
                sb.Append(b.Value ? "true" : "false");
            }
            else if (node is IntNode @int)
            {
                sb.Append("INT,");
                sb.Append(@int.Value.ToString());
            }
            else if (node is FloatNode @float)
            {
                sb.Append("FLOAT,");
                sb.Append(@float.Value.ToString());
            }
            else if (node is InfinityNode inf)
            {
                sb.Append("INF,");
                sb.Append(inf.Positive ? '+' : '-');
            }
            else if (node is NanNode)
                sb.Append("NAN");
            else if (node is CharNode chr)
            {
                sb.Append("CHAR,");
                sb.Append(Pack(chr.Value.ToString()));
            }
            else if (node is StringNode str)
            {
                sb.Append("STR,");
                sb.Append(Pack(str.Value));
            }
            else if (node is DecimalNode dec)
            {
                sb.Append("DEC,");
                sb.Append(dec.Value.ToString());
            }
            else if (node is ColorNode col)
            {
                sb.Append("COL,");
                sb.Append(col.Value.ToString());
            }
            else if (node is TimeNode time)
            {
                sb.Append("TIME,");
                sb.Append(Pack(time.Value.ToString()));
            }
            else if (node is BytesNode bytes)
            {
                sb.Append("BYTES,");
                sb.Append(bytes.Value.ToString());
            }
            else if (node is SymbolNode symbol)
            {
                sb.Append("SYM,");
                sb.Append(Pack(symbol.Name));
            }
            else if (node is RefNode @ref)
            {
                sb.Append("REF,");
                sb.Append(Pack(@ref.ID));
            }
            else if (node is ListNode list)
            {
                sb.Append("LIST\n");
                for (int i = 0; i < list.Count; i++)
                {
                    Serialize(list.GetValueAt(i), sb);
                    sb.Append("\n");
                }
                sb.Append("END");
            }
            else if (node is DictNode dict)
            {
                sb.Append("DICT\n");
                for (int i = 0; i < dict.Count; i++)
                {
                    sb.Append("KEY\n");
                    Serialize(dict.GetKeyAt(i), sb);
                    sb.Append("\nVAL\n");
                    Serialize(dict.GetValueAt(i), sb);
                    sb.Append("\n");
                }
                sb.Append("END");
            }
            else if (node is ObjectNode obj)
            {
                sb.Append("OBJ\n");
                for (int i = 0; i < obj.Count; i++)
                {
                    sb.Append("NAME\n");
                    sb.Append(Pack(obj.GetNameAt(i)));
                    sb.Append("\nVAL\n");
                    Serialize(obj.GetValueAt(i), sb);
                    sb.Append("\n");
                }
                sb.Append("END");
            }
            else
                throw new ArgumentException($"Unexpected node of type {node.GetType()}.");
        }

        private static string Pack(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c == ',' || c == '"' || c == '\n' || c == '\r')
                    return '"' + str.Replace("\"", "\"\"") + '"';
            }
            return str;
        }
    }
}