using System;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

// TODO: this code is bad.

namespace Rusty.Serialization.JSON
{
    public sealed class JsonParser : Parser<JsonLexer>
    {
        private static readonly JsonLexer lexer = new JsonLexer();

        public override NodeTree Parse(TextSpan text, JsonLexer lexer)
        {
            INode root = null;
            while (lexer.GetNextToken(text, out Token token))
            {
                if (root != null)
                    TokenError(token, "Token found after root value.");

                root = ParseToken(text, token, lexer);
            }

            if (root == null)
                TokenError(Token.EOF, "No root value.");

            return new NodeTree(root);
        }


        /* ------------------------------------------------------------ */
        /* Core helpers */

        private static Token Next(ref TextSpan span)
        {
            if (!lexer.GetNextToken(span, out Token token))
                throw new FormatException("Unexpected end of input.");

            // ADVANCE THE SPAN
            span = span.Slice(token.Length);

            return token;
        }

        private static void Expect(ref TextSpan span, string value)
        {
            Token t = Next(ref span);
            if (t.ToString() != value)
                throw new FormatException(
                    $"Expected '{value}', got '{t.ToString()}'.");
        }

        private static bool TryPeek(ref TextSpan span, string value)
        {
            TextSpan copy = span;
            if (!lexer.GetNextToken(copy, out Token token))
                return false;
            return token.ToString() == value;
        }

        private static string ParseString(ref TextSpan span)
        {
            Token t = Next(ref span);
            string s = t.ToString();
            return s.Substring(1, s.Length - 2);
        }

        /* ------------------------------------------------------------ */
        /* Node parsing */

        public static INode ParseNode(ref TextSpan span)
        {
            Expect(ref span, "{");

            string id = null;
            string type = null;
            INode payload = null;

            while (!TryPeek(ref span, "}"))
            {
                UnityEngine.Debug.Log(span.ToString());
                string key = ParseString(ref span);
                Expect(ref span, ":");

                if (key == "$id")
                    id = ParseString(ref span);
                else if (key == "$type")
                    type = ParseString(ref span);
                else
                    payload = ParseTaggedValue(ref span, key);

                if (TryPeek(ref span, ","))
                    Expect(ref span, ",");
            }

            Expect(ref span, "}");

            if (payload == null)
                throw new FormatException("Missing node payload.");

            if (type != null)
                payload = new TypeNode(type, payload);

            if (id != null)
                payload = new IdNode(id, payload);

            return payload;
        }

        /* ------------------------------------------------------------ */
        /* Tagged values */

        private static INode ParseTaggedValue(ref TextSpan span, string tag)
        {
            return tag switch
            {
                "$null" => ParseNull(ref span),
                "$bool" => ParseBool(ref span),
                "$int" => ParseInt(ref span),
                "$float" => ParseFloat(ref span),
                "$inf" => new InfinityNode(true), // TODO: implement
                "$nan" => new NanNode(),
                "$char" => ParseChar(ref span),
                "$str" => new StringNode(ParseString(ref span)),
                "$dec" => ParseDecimal(ref span),
                "$col" => new ColorNode(), // TODO: implement
                "$time" => new TimeNode(), // TODO: implement
                "$bytes" => new BytesNode(), // TODO: implement
                "$symbol" => new SymbolNode(ParseString(ref span)),
                "$ref" => new RefNode(ParseString(ref span)),
                "$list" => ParseList(ref span),
                "$dict" => ParseDict(ref span),
                "$obj" => ParseObject(ref span),
                _ => throw new FormatException($"Unknown node tag '{tag}'.")
            };
        }

        /* ------------------------------------------------------------ */
        /* Primitives */

        private static INode ParseNull(ref TextSpan span)
        {
            Expect(ref span, "null");
            return new NullNode();
        }

        private static INode ParseBool(ref TextSpan span)
        {
            Token t = Next(ref span);
            return new BoolNode(t.ToString() == "true");
        }

        private static INode ParseInt(ref TextSpan span)
            => new IntNode(IntValue.Parse(Next(ref span).ToString()));

        private static INode ParseFloat(ref TextSpan span)
            => new FloatNode(FloatValue.Parse(Next(ref span).ToString()));

        private static INode ParseDecimal(ref TextSpan span)
            => new DecimalNode(DecimalValue.Parse(ParseString(ref span)));

        private static INode ParseChar(ref TextSpan span)
            => new CharNode(ParseString(ref span));

        /* ------------------------------------------------------------ */
        /* Containers */

        private static INode ParseList(ref TextSpan span)
        {
            Expect(ref span, "[");

            ListNode list = new ListNode();

            while (!TryPeek(ref span, "]"))
            {
                list.AddValue(ParseNode(ref span));

                if (TryPeek(ref span, ","))
                    Expect(ref span, ",");
            }

            Expect(ref span, "]");
            return list;
        }

        private static INode ParseDict(ref TextSpan span)
        {
            Expect(ref span, "[");

            DictNode dict = new DictNode();

            while (!TryPeek(ref span, "]"))
            {
                Expect(ref span, "[");

                INode key = ParseNode(ref span);
                Expect(ref span, ",");
                INode value = ParseNode(ref span);

                Expect(ref span, "]");

                dict.AddPair(key, value);

                if (TryPeek(ref span, ","))
                    Expect(ref span, ",");
            }

            Expect(ref span, "]");
            return dict;
        }

        private static INode ParseObject(ref TextSpan span)
        {
            Expect(ref span, "{");

            ObjectNode obj = new ObjectNode();

            while (!TryPeek(ref span, "}"))
            {
                string key = ParseString(ref span);
                Expect(ref span, ":");
                obj.AddMember(key, ParseNode(ref span));

                if (TryPeek(ref span, ","))
                    Expect(ref span, ",");
            }

            Expect(ref span, "}");
            return obj;
        }
    }
}