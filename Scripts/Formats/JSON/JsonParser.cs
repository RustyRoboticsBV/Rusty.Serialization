using System;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.JSON
{
    public sealed class JsonParser : Parser<JsonLexer>
    {
        /* Public methods. */
        public override NodeTree Parse(TextSpan text, JsonLexer lexer)
        {
            JsonNode root = ParseAsJsonNode(text, lexer);

            if (lexer.GetNextToken(text, out Token token))
                TokenError(token, "Unexpected token after root value.");

            if (root == null)
                TokenError(Token.EOF, "No root value.");

            NodeTree tree = new NodeTree(ParseAsNode(root));
            return tree;
        }

        /* Private methods. */
        private static JsonNode ParseAsJsonNode(TextSpan text, JsonLexer lexer)
        {
            if (!lexer.GetNextToken(text, out Token token))
                throw new FormatException("Unexpected EOF.");
            return ParseAsJsonNode(text, lexer, token);
        }

        private static JsonNode ParseAsJsonNode(TextSpan text, JsonLexer lexer, Token token)
        {
            if (token.Text.Equals("null"))
                return new JsonNull();
            if (token.Text.Equals("true"))
                return new JsonBoolean(true);
            if (token.Text.Equals("false"))
                return new JsonBoolean(false);
            if (token.Text.EnclosedWith('"'))
                return new JsonString(token.Text.Slice(1, token.Text.Length - 2));
            if (token.Text.Equals("["))
                return ParseAsJsonArray(text, lexer);
            if (token.Text.Equals("{"))
                return ParseAsJsonObject(text, lexer);
            if (token.Text.StartsWith('-') || token.Text.StartsWithDigit())
                return new JsonNumber(double.Parse(token.Text));
            throw new FormatException($"Invalid token '{token.Text}'.");
        }

        private static JsonArray ParseAsJsonArray(TextSpan text, JsonLexer lexer)
        {
            JsonArray array = new JsonArray();

            while (true)
            {
                Token token = ExpectToken(text, lexer, "Unexpected end of file (unclosed list).");
                if (token.Text.Equals(']'))
                    break;

                if (array.Count > 0)
                {
                    MustEqual(token, ',', "List elements must be separated with a comma.");
                    token = ExpectToken(text, lexer, "Unexpected end of file (unclosed object).");
                }

                array.Add(ParseAsJsonNode(text, lexer, token));
            }

            return array;
        }

        private static JsonObject ParseAsJsonObject(TextSpan text, JsonLexer lexer)
        {
            JsonObject obj = new JsonObject();

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

                JsonNode key = ParseAsJsonNode(text, lexer, token);
                ExpectSymbol(text, lexer, ':', "Object keys and values must be separated with a colon.");
                JsonNode value = ParseAsJsonNode(text, lexer);

                if (!(key is JsonString keyStr))
                    throw new FormatException("Object keys must be strings.");
                obj.Add(keyStr.value, value);
            }

            return obj;
        }


        private static INode ParseAsNode(JsonNode node)
        {
            if (node is JsonNull)
                return new NullNode();
            if (node is JsonBoolean boolean)
                return new BoolNode(boolean.value);
            if (node is JsonNumber number)
                return new FloatNode(number.value);
            if (node is JsonString text)
                return new StringNode(text.value);
            if (node is JsonArray array)
            {
                ListNode list = new ListNode();
                for (int i = 0; i < array.Count; i++)
                {
                    list.AddValue(ParseAsNode(array.values[i]));
                }
                return list;
            }
            if (node is JsonObject obj)
            {
                // If a container, parse as such.
                for (int i = 0; i < obj.Count; i++)
                {
                    string key = obj.keys[i];
                    if (key.StartsWith('$') && (key == "$id" || key == "$type" || key == "$scope" || key == "$value"))
                    {
                        ParseContainer(obj, out INode value, out string scope);
                        return value;
                    }
                }

                // Else, parse as an object.
                throw new NotImplementedException(); // TODO
            }
            throw new FormatException("Unknown JSON node.");
        }

        private static void ParseContainer(JsonObject json, out INode node, out string scope)
        {
            node = null;
            string address = null;
            string type = null;
            scope = null;
            for (int i = 0; i < json.Count; i++)
            {
                string key = json.keys[i];
                if (key == "$id")
                {
                    if (json.values[i] is JsonString str)
                        address = str.value;
                    else
                        throw new FormatException("$id keys must be followed by a string value.");
                }
                if (key == "$type")
                {
                    if (json.values[i] is JsonString str)
                        type = str.value;
                    else
                        throw new FormatException("$type keys must be followed by a string value.");
                }
                if (key == "$scope")
                {
                    if (json.values[i] is JsonString str)
                        scope = str.value;
                    else
                        throw new FormatException("$scope keys must be followed by a string value.");
                }
                if (key == "$value")
                    node = ParseAsNode(json.values[i]);
            }

            if (node == null)
                throw new FormatException("Missing $value key.");
            if (type != null)
                node = new TypeNode(type, node);
            if (address != null)
                node = new AddressNode(address, node);
        }
    }
}