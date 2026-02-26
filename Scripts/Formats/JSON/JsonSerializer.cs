using System;
using System.Text;
using System.Numerics;
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
            JsonNode root = ToJsonNodes(node.Root);
            StringBuilder sb = new StringBuilder(); // TODO: use rented bag sb instead.
            Serialize(sb, root);
            return sb.ToString();
        }

        /* Private methods. */
        private static JsonNode ToJsonNodes(INode node)
        {
            // Unwrap metadata.
            string addressStr = null;
            if (node is AddressNode id)
            {
                addressStr = id.Name;
                node = id.Value;
            }

            string typeStr = null;
            if (node is TypeNode type)
            {
                typeStr = type.Name;
                node = type.Value;
            }

            string scopeStr = null;
            if (node is ScopeNode scope)
            {
                scopeStr = scope.Name;
                node = scope.Value;
            }

            string offsetStr = null;
            if (node is OffsetNode offset)
            {
                offsetStr = offset.Offset.ToString();
                node = offset.Time;
            }


            bool mustWrap = addressStr != null || typeStr != null || offsetStr != null || scopeStr != null;

            JsonNode result;
            switch (node)
            {
                case NullNode:
                    result = new JsonNull();
                    break;

                case BoolNode @bool:
                    result = new JsonBoolean(@bool.Value);
                    break;

                case IntNode @int:
                    result = new JsonNumber((double)(BigInteger)@int.Value);
                    break;

                case FloatNode @float:
                    result = new JsonNumber((double)@float.Value);
                    break;

                case InfinityNode infinity:
                    if (infinity.Positive)
                        result = new JsonString("Infinity");
                    else
                        result = new JsonString("-Infinity");
                    break;

                case NanNode:
                    result = new JsonString("NaN");
                    break;

                case CharNode @char:
                    result = new JsonString(@char.Value.ToString());
                    break;

                case StringNode @string:
                    result = new JsonString(@string.Value);
                    break;

                case DecimalNode @decimal:
                    result = new JsonString(@decimal.Value.ToString());
                    break;

                case ColorNode color:
                    result = new JsonString(color.Value.ToString());
                    break;

                case UidNode uid:
                    result = new JsonString(uid.Value.ToString());
                    break;

                case TimestampNode timestamp:
                    result = new JsonString(timestamp.Value.ToString());
                    break;

                case DurationNode duration:
                    result = new JsonString(duration.Value.ToString());
                    break;

                case BytesNode bytes:
                    result = new JsonString(bytes.Value.ToString());
                    break;

                case SymbolNode symbol:
                    result = new JsonString(symbol.Name);
                    break;

                case RefNode @ref:
                    result = new JsonString(@ref.Address);
                    break;

                case ListNode list:
                    JsonArray arrayList = new JsonArray();
                    for (int i = 0; i < list.Count; i++)
                    {
                        arrayList.Add(ToJsonNodes(list.GetValueAt(i)));
                    }
                    result = arrayList;
                    break;

                case DictNode dict:
                    JsonArray arrayDict = new JsonArray();
                    for (int i = 0; i < dict.Count; i++)
                    {
                        JsonArray member = new JsonArray();
                        member.Add(ToJsonNodes(dict.GetKeyAt(i)));
                        member.Add(ToJsonNodes(dict.GetValueAt(i)));
                        arrayDict.Add(member);
                    }
                    result = arrayDict;
                    break;

                case ObjectNode obj:
                    JsonArray arrayObj = new JsonArray();
                    for (int i = 0; i < obj.Count; i++)
                    {
                        JsonArray member = new JsonArray();
                        member.Add(ToJsonNodes(obj.GetNameAt(i)));
                        member.Add(ToJsonNodes(obj.GetValueAt(i)));
                        arrayObj.Add(member);
                    }
                    result = arrayObj;
                    break;

                default:
                    throw new ArgumentException($"Invalid node type '{node.GetType()}'.");
            }

            // Wrap if necessary.
            if (mustWrap)
            {
                JsonObject wrapper = new JsonObject();

                // Address.
                if (addressStr != null)
                    wrapper.Add("$id", new JsonString(addressStr));

                // Type.
                if (typeStr != null)
                    wrapper.Add("$type", new JsonString(typeStr));

                // Scope.
                if (scopeStr != null)
                    wrapper.Add("$scope", new JsonString(scopeStr));

                // Value.
                wrapper.Add("$value", result);

                return wrapper;
            }

            // Else, return as-is.
            else
                return result;
        }

        private static void Serialize(StringBuilder sb, JsonNode node, int indentation = 0, bool indentStart = true)
        {
            if (indentStart)
                Indent(sb, indentation);
            switch (node)
            {
                case JsonNull:
                    sb.Append("null");
                    break;
                case JsonBoolean boolean:
                    sb.Append(boolean.value ? "true" : "false");
                    break;
                case JsonNumber number:
                    sb.Append(number.value.ToString());
                    break;
                case JsonString text:
                    sb.Append('"');
                    sb.Append(text.value);
                    sb.Append('"');
                    break;
                case JsonArray array:
                    sb.Append("[\n");
                    for (int i = 0; i < array.values.Count; i++)
                    {
                        if (i > 0)
                            sb.Append(",\n");
                        Serialize(sb, array.values[i], indentation + 1);
                    }
                    sb.Append("\n");
                    Indent(sb, indentation);
                    sb.Append("]");
                    break;
                case JsonObject obj:
                    sb.Append("{\n");
                    for (int i = 0; i < obj.Count; i++)
                    {
                        if (i > 0)
                            sb.Append(",\n");
                        Indent(sb, indentation + 1);
                        sb.Append('"');
                        sb.Append(obj.keys[i]);
                        sb.Append('"');
                        sb.Append(": ");
                        Serialize(sb, obj.values[i], indentation + 1, false);
                    }
                    sb.Append("\n");
                    Indent(sb, indentation);
                    sb.Append("}");
                    break;
                default:
                    throw new FormatException("Unknown node type.");
            }
        }

        private static void Indent(StringBuilder sb, int indentation)
        {
            if (indentation == 0)
                return;

            for (int j = 0; j < indentation; j++)
            {
                sb.Append("  ");
            }
        }
    }
}