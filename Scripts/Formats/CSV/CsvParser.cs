using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSV
{
    public static class CsvParser
    {
        /* Public methods. */
        public static NodeTree Parse(string serialized)
        {
            CsvLexer lexer = new CsvLexer(serialized);
            INode root = ParseNode(lexer);
            return new NodeTree(root);
        }

        /* Private methods. */
        private static INode ParseNode(CsvLexer lexer)
        {
            if (!lexer.GetNextCell(out var cell, out bool _))
                throw new FormatException("Unexpected end of CSV.");

            string token = Unpack(cell);
            UnityEngine.Debug.Log(token);

            switch (token)
            {
                case "key":
                case "name":
                case "value":
                case "end":
                    return null;

                case "null":
                    return new NullNode();

                case "bool":
                    return new BoolNode(ReadCell(lexer) == "true");

                case "int":
                    return new IntNode(IntValue.Parse(ReadCell(lexer)));

                case "float":
                    return new FloatNode(FloatValue.Parse(ReadCell(lexer)));

                case "inf":
                    return new InfinityNode(ReadCell(lexer) == "+");

                case "nan":
                    return new NanNode();

                case "char":
                    return new CharNode(ReadCell(lexer));

                case "str":
                    return new StringNode(ReadCell(lexer));

                case "dec":
                    return new DecimalNode(DecimalValue.Parse(ReadCell(lexer)));

                case "time":
                    return new TimestampNode(TimestampValue.Parse(ReadCell(lexer)));

                case "bytes":
                    return new BytesNode(BytesValue.Parse(ReadCell(lexer)));

                case "sym":
                    return new SymbolNode(ReadCell(lexer));

                case "ref":
                    return new RefNode(ReadCell(lexer));

                case "id":
                    string id = ReadCell(lexer);
                    INode idValue = ParseNode(lexer);
                    return new IdNode(id, idValue);

                case "type":
                    string type = ReadCell(lexer);
                    INode typeValue = ParseNode(lexer);
                    return new IdNode(type, typeValue);

                case "list":
                    ListNode list = new ListNode();
                    while (true)
                    {
                        INode element = ParseNode(lexer);
                        if (element == null)
                            break;
                        list.AddValue(element);
                    }
                    return list;

                case "dict":
                    DictNode dict = new DictNode();
                    while (true)
                    {
                        try
                        {
                            Expect(lexer, "key");
                            INode key = ParseNode(lexer);

                            Expect(lexer, "val");
                            INode value = ParseNode(lexer);

                            dict.AddPair(key, value);
                        }
                        catch
                        {
                            break;
                        }
                    }
                    return dict;

                case "obj":
                    ObjectNode obj = new ObjectNode();
                    while (true)
                    {
                        try
                        {
                            Expect(lexer, "name");
                            IMemberNameNode name = ParseNode(lexer) as IMemberNameNode;

                            Expect(lexer, "val");
                            INode value = ParseNode(lexer);

                            obj.AddMember(name, value);
                        }
                        catch
                        {
                            break;
                        }
                    }
                    return obj;

                default:
                    throw new FormatException($"Unknown token '{token}'.");
            }
        }

        private static string ReadCell(CsvLexer lexer)
        {
            ReadCell(lexer, out string str, out _);
            return str;
        }

        private static void ReadCell(CsvLexer lexer, out string value, out bool eol)
        {
            if (!lexer.GetNextCell(out var cell, out eol))
                throw new FormatException("Expected cell.");
            value = Unpack(cell);
        }

        private static void Expect(CsvLexer lexer, string expected)
        {
            if (!lexer.GetNextCell(out var cell, out _))
                throw new FormatException($"Expected '{expected}'.");
            if (Unpack(cell) != expected)
                throw new FormatException($"Expected '{expected}'.");
        }

        private static string Unpack(ReadOnlySpan<char> span)
        {
            if (span.Length >= 2 && span[0] == '"' && span[span.Length - 1] == '"')
                return span.Slice(1, span.Length - 2).ToString().Replace("\"\"", "\"");
            return span.ToString();
        }
    }
}