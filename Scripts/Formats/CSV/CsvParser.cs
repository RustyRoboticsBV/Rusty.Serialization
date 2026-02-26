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
            INode root = ParseNode(ref lexer);
            NodeTree tree = new NodeTree(root);
#if UNITY_5_3_OR_NEWER
            UnityEngine.Debug.Log(tree);
#endif
            return tree;
        }

        /* Private methods. */
        private static INode ParseNode(ref CsvLexer lexer)
        {
            if (!lexer.GetNextCell(out var cell, out bool _))
                throw new FormatException("Unexpected end of CSV.");

            string token = Unpack(cell);

            switch (token)
            {
                case "end":
                    return null;

                case "adr":
                    string address = ReadCell(ref lexer);
                    INode addressedValue = ParseNode(ref lexer);
                    return new AddressNode(address, addressedValue);

                case "type":
                    string type = ReadCell(ref lexer);
                    INode typedValue = ParseNode(ref lexer);
                    return new TypeNode(type, typedValue);

                case "scope":
                    string scope = ReadCell(ref lexer);
                    SymbolNode scopedValue = ParseNode(ref lexer) as SymbolNode;
                    return new ScopeNode(scope, scopedValue);

                case "offset":
                    string offset = ReadCell(ref lexer);
                    TimestampNode offsetValue = ParseNode(ref lexer) as TimestampNode;
                    return new OffsetNode(OffsetValue.Parse(offset), offsetValue);

                case "null":
                    return new NullNode();

                case "bool":
                    return new BoolNode(ReadCell(ref lexer) == "true");

                case "int":
                    return new IntNode(IntValue.Parse(ReadCell(ref lexer)));

                case "float":
                    return new FloatNode(FloatValue.Parse(ReadCell(ref lexer)));

                case "inf":
                    return new InfinityNode(ReadCell(ref lexer) == "+");

                case "nan":
                    return new NanNode();

                case "char":
                    return new CharNode(ReadCell(ref lexer));

                case "str":
                    return new StringNode(ReadCell(ref lexer));

                case "dec":
                    return new DecimalNode(DecimalValue.Parse(ReadCell(ref lexer)));

                case "col":
                    return new ColorNode(ColorValue.Parse(ReadCell(ref lexer)));

                case "uid":
                    return new UidNode(Guid.Parse(ReadCell(ref lexer)));

                case "time":
                    return new TimestampNode(TimestampValue.Parse(ReadCell(ref lexer)));

                case "dur":
                    return new DurationNode(DurationValue.Parse(ReadCell(ref lexer)));

                case "bytes":
                    return new BytesNode(BytesValue.Parse(ReadCell(ref lexer)));

                case "sym":
                    return new SymbolNode(ReadCell(ref lexer));

                case "ref":
                    return new RefNode(ReadCell(ref lexer));

                case "list":
                    ListNode list = new ListNode();
                    while (true)
                    {
                        INode element = ParseNode(ref lexer);
                        if (element == null)
                            break;

                        list.AddValue(element);
                    }
                    return list;

                case "dict":
                    DictNode dict = new DictNode();
                    while (true)
                    {
                        INode key = ParseNode(ref lexer);
                        if (key == null)
                            break;

                        INode value = ParseNode(ref lexer);

                        dict.AddPair(key, value);
                    }
                    return dict;

                case "obj":
                    ObjectNode obj = new ObjectNode();
                    while (true)
                    {
                        INode next = ParseNode(ref lexer);
                        if (next == null)
                            break;

                        if (!(next is IMemberNameNode name))
                            throw new FormatException();

                        INode value = ParseNode(ref lexer);

                        obj.AddMember(name, value);
                    }
                    return obj;

                default:
                    throw new FormatException($"Unknown token '{token}'.");
            }
        }

        private static string ReadCell(ref CsvLexer lexer)
        {
            ReadCell(ref lexer, out string str, out _);
            return str;
        }

        private static void ReadCell(ref CsvLexer lexer, out string value, out bool eol)
        {
            if (!lexer.GetNextCell(out var cell, out eol))
                throw new FormatException("Expected cell.");
            value = Unpack(cell);
        }

        private static void Expect(ref CsvLexer lexer, string expected)
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