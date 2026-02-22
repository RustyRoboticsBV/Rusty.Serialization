using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.JSON
{
    public sealed class JsonParser : Parser<JsonLexer>
    {
        private static readonly JsonLexer lexer = new JsonLexer();

        public override NodeTree Parse(TextSpan text, JsonLexer lexer)
        {
            ExpectSymbol(text, lexer, '{', "No opening { for node.");

            INode root = ParseNode(text, lexer);

            if (lexer.GetNextToken(text, out Token token))
                TokenError(token, "Unexpected token after root value.");

            if (root == null)
                TokenError(Token.EOF, "No root value.");

            NodeTree tree = new NodeTree(root);
            return tree;
        }

        /* Private methods. */
        private static INode ParseNode(TextSpan text, JsonLexer lexer)
        {
            INode node = null;

            if (!lexer.GetNextToken(text, out Token token))
                TokenError(Token.EOF, "Unexpected end of file.");

            if (!token.Text.EnclosedWith('"'))
                TokenError(token, "Missing node tag.");

            TextSpan tag = token.Text.Slice(1, token.Text.Length - 2);

            string id = null;
            if (tag.Equals("$id"))
            {
                Token name = GetStringValue(text, lexer);
                id = ParseString(name);
                ExpectSymbol(text, lexer, ',', "Missing comma after id.");
                token = ExpectToken(text, lexer, "Unexpected end of file after id.");
                tag = token.Text.Slice(1, token.Text.Length - 2);
            }

            string type = null;
            if (tag.Equals("$type"))
            {
                Token name = GetStringValue(text, lexer);
                type = ParseString(name);
                ExpectSymbol(text, lexer, ',', "Missing comma after type.");
                token = ExpectToken(text, lexer, "Unexpected end of file after type.");
                tag = token.Text.Slice(1, token.Text.Length - 2);
            }

            if (tag.Equals("$null"))
            {
                GetValue(text, lexer);
                node = new NullNode();
            }
            else if (tag.Equals("$bool"))
            {
                Token value = GetBoolValue(text, lexer);
                node = new BoolNode(value.Text.Length == 4);
            }
            else if (tag.Equals("$int"))
            {
                Token value = GetValue(text, lexer);
                node = new IntNode(IntValue.Parse(value.ToString()));
            }
            else if (tag.Equals("$float"))
            {
                Token value = GetValue(text, lexer);
                node = new FloatNode(FloatValue.Parse(value.ToString()));
            }
            else if (tag.Equals("$inf"))
            {
                Token value = GetInfinityValue(text, lexer);
                if (ParseString(value) == "+")
                    node = new InfinityNode(true);
                else
                    node = new InfinityNode(false);
            }
            else if (tag.Equals("$nan"))
            {
                GetValue(text, lexer);
                node = new NanNode();
            }
            else if (tag.Equals("$char"))
            {
                Token value = GetStringValue(text, lexer);
                node = new CharNode(ParseString(value));
            }
            else if (tag.Equals("$str"))
            {
                Token value = GetStringValue(text, lexer);
                node = new StringNode(ParseString(value));
            }
            else if (tag.Equals("$dec"))
            {
                Token value = GetValue(text, lexer);
                node = new DecimalNode(DecimalValue.Parse(value.ToString()));
            }
            else if (tag.Equals("$col"))
            {
                Token value = GetStringValue(text, lexer);
                node = new ColorNode(ColorValue.Parse(ParseString(value)));
            }
            else if (tag.Equals("$time"))
            {
                Token value = GetStringValue(text, lexer);
                node = new TimestampNode(TimestampValue.Parse(ParseString(value)));
            }
            else if (tag.Equals("$bytes"))
            {
                Token value = GetStringValue(text, lexer);
                node = new BytesNode(BytesValue.Parse(ParseString(value)));
            }
            else if (tag.Equals("$symbol"))
            {
                Token value = GetStringValue(text, lexer);
                node = new SymbolNode(ParseString(value));
            }
            else if (tag.Equals("$ref"))
            {
                Token value = GetStringValue(text, lexer);
                node = new RefNode(ParseString(value));
            }
            else if (tag.Equals("$list"))
                node = ParseList(text, lexer);
            else if (tag.Equals("$dict"))
                node = ParseDict(text, lexer);
            else if (tag.Equals("$obj"))
                node = ParseObject(text, lexer);
            else
                TokenError(token, $"Unrecognized node tag {tag.ToString()}");

            ExpectSymbol(text, lexer, '}', "Expected closing } after " + new string(tag) + " node contents.");

            if (type != null)
                node = new TypeNode(type, node);

            if (id != null)
                node = new AddressNode(id, node);
            return node;
        }

        private static Token GetValue(TextSpan text, JsonLexer lexer)
        {
            ExpectSymbol(text, lexer, ':', "Expected : symbol before value.");

            if (!lexer.GetNextToken(text, out Token token))
                TokenError(Token.EOF, "Missing value after : symbol.");

            return token;
        }

        private static Token GetBoolValue(TextSpan text, JsonLexer lexer)
        {
            Token value = GetValue(text, lexer);

            if (!value.Text.Equals("true") && !value.Text.Equals("false"))
                TokenError(value, "Expected a boolean.");

            return value;
        }

        private static Token GetStringValue(TextSpan text, JsonLexer lexer)
        {
            Token value = GetValue(text, lexer);

            if (!value.Text.EnclosedWith('"'))
                TokenError(value, "Expected a string.");

            return value;
        }

        private static Token GetInfinityValue(TextSpan text, JsonLexer lexer)
        {
            Token value = GetStringValue(text, lexer);

            if (!value.Text.Equals("\"+\"") && !value.Text.Equals("\"-\""))
                TokenError(value, "Expected a + or - character.");

            return value;
        }

        private static string ParseString(Token token)
        {
            if (!token.Text.EnclosedWith('"'))
                TokenError(token, "Expected a string.");
            return token.Text.Slice(1, token.Length - 2).ToString();
        }

        private static ListNode ParseList(TextSpan text, JsonLexer lexer)
        {
            ExpectSymbol(text, lexer, ':', "Expected ':' before list.");
            ExpectSymbol(text, lexer, '[', "Expected list to start with '['.");

            ListNode list = new ListNode();

            while (true)
            {
                Token token = ExpectToken(text, lexer, "Unexpected end of file (unclosed list).");
                if (token.Text.Equals(']'))
                    break;

                if (list.Count > 0)
                {
                    MustEqual(token, ',', "List elements must be separated with a comma.");
                    token = ExpectToken(text, lexer, "Unexpected end of file (unclosed object).");
                }

                MustEqual(token, '{', "Expected start of node using a { symbol.");
                list.AddValue(ParseNode(text, lexer));
            }

            return list;
        }

        private static DictNode ParseDict(TextSpan text, JsonLexer lexer)
        {
            ExpectSymbol(text, lexer, ':', "Expected : symbol before dictionary.");
            ExpectSymbol(text, lexer, '[', "Expected dictionary to start with [ symbol.");

            DictNode dict = new DictNode();

            while (true)
            {
                Token token = ExpectToken(text, lexer, "Unexpected end of file (unclosed dictionary).");
                if (token.Text.Equals(']'))
                    break;

                if (dict.Count > 0)
                {
                    MustEqual(token, ',', "Dictionary elements must be separated with a comma.");
                    token = ExpectToken(text, lexer, "Unexpected end of file (unclosed object).");
                }

                MustEqual(token, '[', "Dictionary entries must start with a [ symbol.");

                ExpectSymbol(text, lexer, '{', "Expected start of key node using a { symbol.");
                INode key = ParseNode(text, lexer);

                ExpectSymbol(text, lexer, ',', "Dictionary keys and values must be separated with a comma.");

                ExpectSymbol(text, lexer, '{', "Expected start of value node using a { symbol.");
                INode value = ParseNode(text, lexer);

                ExpectSymbol(text, lexer, ']', "Dictionary entries must end with a ] symbol.");

                dict.AddPair(key, value);
            }

            return dict;
        }

        private static ObjectNode ParseObject(TextSpan text, JsonLexer lexer)
        {
            ExpectSymbol(text, lexer, ':', "Expected : symbol before object.");
            ExpectSymbol(text, lexer, '{', "Expected object to start with { symbol.");

            ObjectNode obj = new ObjectNode();

            while (true)
            {
                Token token = ExpectToken(text, lexer, "Unexpected end of file (unclosed object).");
                if (token.Text.Equals('}'))
                    break;

                if (obj.Count > 0)
                {
                    MustEqual(token, ',', "Object members must be separated with a comma.");
                    token = ExpectToken(text, lexer, "Unexpected end of file (unclosed object).");
                }

                Token key = token;

                ExpectSymbol(text, lexer, ':', "Object member names and values must be separated with a colon.");

                ExpectSymbol(text, lexer, '{', "Expected object member value to start with a left curly-brace.");
                INode value = ParseNode(text, lexer);

                obj.AddMember(null/*ParseString(key)*/, value); // TODO: fix.
            }

            return obj;
        }
    }
}