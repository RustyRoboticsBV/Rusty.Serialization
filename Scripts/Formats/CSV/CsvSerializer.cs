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
            if (node is AddressNode address)
            {
                sb.Append("id,");
                sb.Append(Pack(address.Name));
                sb.Append("\n");
                Serialize(address.Value, sb);
                sb.Append("\nend");
            }
            else if (node is TypeNode type)
            {
                sb.Append("type,");
                sb.Append(Pack(type.Name));
                sb.Append("\n");
                Serialize(type.Value, sb);
                sb.Append("\nend");
            }
            else if (node is NullNode)
                sb.Append("null");
            else if (node is BoolNode b)
            {
                sb.Append("bool,");
                sb.Append(b.Value ? "true" : "false");
            }
            else if (node is IntNode @int)
            {
                sb.Append("int,");
                sb.Append(@int.Value.ToString());
            }
            else if (node is FloatNode @float)
            {
                sb.Append("float,");
                sb.Append(@float.Value.ToString());
            }
            else if (node is InfinityNode inf)
            {
                sb.Append("inf,");
                sb.Append(inf.Positive ? '+' : '-');
            }
            else if (node is NanNode)
                sb.Append("nan");
            else if (node is CharNode chr)
            {
                sb.Append("char,");
                sb.Append(Pack(chr.Value.ToString()));
            }
            else if (node is StringNode str)
            {
                sb.Append("str,");
                sb.Append(Pack(str.Value));
            }
            else if (node is DecimalNode dec)
            {
                sb.Append("dec,");
                sb.Append(dec.Value.ToString());
            }
            else if (node is ColorNode col)
            {
                sb.Append("col,");
                sb.Append(col.Value.ToString());
            }
            else if (node is TimestampNode time)
            {
                sb.Append("time,");
                sb.Append(Pack(time.Value.ToString()));
            }
            else if (node is BytesNode bytes)
            {
                sb.Append("bytes,");
                sb.Append(bytes.Value.ToString());
            }
            else if (node is SymbolNode symbol)
            {
                sb.Append("sym,");
                sb.Append(Pack(symbol.Name));
            }
            else if (node is RefNode @ref)
            {
                sb.Append("ref,");
                sb.Append(Pack(@ref.Address));
            }
            else if (node is ListNode list)
            {
                sb.Append("list\n");
                for (int i = 0; i < list.Count; i++)
                {
                    Serialize(list.GetValueAt(i), sb);
                    sb.Append("\n");
                }
                sb.Append("end");
            }
            else if (node is DictNode dict)
            {
                sb.Append("dict\n");
                for (int i = 0; i < dict.Count; i++)
                {
                    sb.Append("key\n");
                    Serialize(dict.GetKeyAt(i), sb);
                    sb.Append("\nval\n");
                    Serialize(dict.GetValueAt(i), sb);
                    sb.Append("\n");
                }
                sb.Append("end");
            }
            else if (node is ObjectNode obj)
            {
                sb.Append("obj\n");
                for (int i = 0; i < obj.Count; i++)
                {
                    sb.Append("name,");
                    sb.Append(Pack(obj.GetNameAt(i).ToString()));
                    sb.Append("\nval\n");
                    Serialize(obj.GetValueAt(i), sb);
                    sb.Append("\n");
                }
                sb.Append("end");
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