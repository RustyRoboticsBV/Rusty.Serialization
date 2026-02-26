using System;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

// TODO: replace gross and bad XDocument usage with a more performant implementation. 

namespace Rusty.Serialization.XML
{
    /// <summary>
    /// A XML serializer/deserializer back-end.
    /// </summary>
    public class XmlCodec : Codec
    {
        /* Public methods. */
        public override string Serialize(NodeTree node, Settings settings)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = !settings.IncludeFormatHeader,
                Indent = settings.PrettyPrint,
                IndentChars = "  ",
                NewLineHandling = NewLineHandling.Replace
            };

            using StringWriter sw = new StringWriter();
            using XmlWriter writer = XmlWriter.Create(sw, xmlSettings);

            WriteNode(writer, node.Root);

            writer.Flush();
            return sw.ToString();
        }

        public override NodeTree Parse(string serialized)
        {
            XDocument doc = XDocument.Parse(serialized, LoadOptions.PreserveWhitespace);
            XElement rootElement = doc.Root;
            if (rootElement == null)
                throw new ArgumentException("XML does not contain a root element.");
            INode rootNode = ReadNode(rootElement);
            NodeTree tree = new NodeTree(rootNode);
#if UNITY_5_3_OR_NEWER
            UnityEngine.Debug.Log(tree);
#endif
            return tree;
        }

        /* Private methods. */

        // Writing.

        private static void WriteNode(XmlWriter writer, INode node, string name = "", string scope = "")
        {
            // Unwrap address node.
            string addressName = null;
            if (node is AddressNode address)
            {
                addressName = address.Name;
                node = address.Value;
            }

            // Unwrap type node.
            string typeName = null;
            if (node is TypeNode type)
            {
                typeName = type.Name;
                node = type.Value;
            }

            // Unwrap offset node.
            string offsetValue = null;
            if (node is OffsetNode offset)
            {
                offsetValue = offset.Offset.ToString();
                node = offset.Time;
            }

            // Write node.
            switch (node)
            {
                case NullNode:
                    writer.WriteStartElement("null");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteEndElement();
                    break;

                case BoolNode @bool:
                    writer.WriteStartElement("bool");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(@bool.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case IntNode @int:
                    writer.WriteStartElement("int");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(@int.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case FloatNode @float:
                    writer.WriteStartElement("float");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(@float.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case NanNode:
                    writer.WriteStartElement("nan");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteEndElement();
                    break;

                case InfinityNode inf:
                    writer.WriteStartElement("inf");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);
                    if (inf.Positive)
                        writer.WriteString("+");
                    else
                        writer.WriteString("-");

                    writer.WriteEndElement();
                    break;

                case CharNode chr:
                    writer.WriteStartElement("char");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(chr.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case StringNode str:
                    writer.WriteStartElement("str");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(str.Value);
                    writer.WriteEndElement();
                    break;

                case DecimalNode @decimal:
                    writer.WriteStartElement("dec");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(@decimal.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case ColorNode color:
                    writer.WriteStartElement("col");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(color.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case UidNode uid:
                    writer.WriteStartElement("uid");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(uid.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case TimestampNode timestamp:
                    writer.WriteStartElement("time");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    if (timestamp.Value.year != 1)
                        writer.WriteElementString("year", timestamp.Value.year.ToString());
                    if (timestamp.Value.month != 1)
                        writer.WriteElementString("month", timestamp.Value.month.ToString());
                    if (timestamp.Value.day != 1)
                        writer.WriteElementString("day", timestamp.Value.day.ToString());
                    if (timestamp.Value.hour != 0)
                        writer.WriteElementString("hour", timestamp.Value.hour.ToString());
                    if (timestamp.Value.minute != 0)
                        writer.WriteElementString("minute", timestamp.Value.minute.ToString());
                    if (timestamp.Value.second != 0)
                        writer.WriteElementString("second", timestamp.Value.second.ToString());

                    writer.WriteEndElement();
                    break;

                case DurationNode duration:
                    writer.WriteStartElement("dur");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    if (duration.Value.negative)
                        writer.WriteElementString("negative", duration.Value.negative.ToString());
                    if (duration.Value.days != 0)
                        writer.WriteElementString("days", duration.Value.days.ToString());
                    if (duration.Value.hours != 0)
                        writer.WriteElementString("hours", duration.Value.hours.ToString());
                    if (duration.Value.minutes != 0)
                        writer.WriteElementString("minutes", duration.Value.minutes.ToString());
                    if (duration.Value.seconds != 0)
                        writer.WriteElementString("seconds", duration.Value.seconds.ToString());

                    writer.WriteEndElement();
                    break;

                case BytesNode bytes:
                    writer.WriteStartElement("bytes");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(bytes.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case SymbolNode symbol:
                    writer.WriteStartElement("memberSymbol");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(symbol.Name);
                    writer.WriteEndElement();
                    break;

                case RefNode @ref:
                    writer.WriteStartElement("ref");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    writer.WriteString(@ref.Address);
                    writer.WriteEndElement();
                    break;

                case ListNode list:
                    writer.WriteStartElement("list");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    foreach (var element in list.Elements)
                    {
                        WriteNode(writer, element);
                    }
                    writer.WriteEndElement();
                    break;

                case DictNode dict:
                    writer.WriteStartElement("dict");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    foreach (var pair in dict.Pairs)
                    {
                        writer.WriteStartElement("entry");
                        writer.WriteStartElement("key");
                        WriteNode(writer, pair.Key);
                        writer.WriteEndElement();
                        writer.WriteStartElement("value");
                        WriteNode(writer, pair.Value);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    break;

                case ObjectNode obj:
                    writer.WriteStartElement("obj");
                    WriteMetadata(writer, addressName, typeName, offsetValue, name, scope);

                    foreach (var member in obj.Members)
                    {
                        string scopeName = "";
                        string memberName = "";
                        if (member.Key is ScopeNode memberScope)
                        {
                            scopeName = memberScope.Name;
                            memberName = memberScope.Value.Name;
                        }
                        else if (member.Key is SymbolNode memberSymbol)
                            memberName = memberSymbol.Name;

                        WriteNode(writer, member.Value, memberName, scopeName);
                    }
                    writer.WriteEndElement();
                    break;

                default:
                    throw new NotSupportedException($"Unsupported node type {node.GetType().Name}");
            }
        }

        private static void WriteMetadata(XmlWriter writer, string address, string type, string offset, string name, string scope)
        {
            if (!string.IsNullOrEmpty(address))
                writer.WriteAttributeString("id", address);
            if (!string.IsNullOrEmpty(type))
                writer.WriteAttributeString("type", type);
            if (!string.IsNullOrEmpty(offset))
                writer.WriteAttributeString("offset", offset);
            if (!string.IsNullOrEmpty(name))
                writer.WriteAttributeString("name", name);
            if (!string.IsNullOrEmpty(scope))
                writer.WriteAttributeString("scope", scope);
        }


        // Parsing.

        private static bool IsPrimitive(string tag) => tag == "null" || tag == "bool" || tag == "int" || tag == "float"
            || tag == "inf" || tag == "nan" || tag == "char" || tag == "str" || tag == "dec" || tag == "col" || tag == "uid"
            || tag == "bytes" || tag == "memberSymbol" || tag == "ref";

        private static INode ParsePrimitive(string tag, string value) => tag switch
        {
            "null" => new NullNode(),
            "bool" => new BoolNode(BoolValue.Parse(value)),
            "int" => new IntNode(IntValue.Parse(value)),
            "float" => new FloatNode(FloatValue.Parse(value)),
            "inf" => value == "+" ? new InfinityNode(true) : new InfinityNode(false),
            "nan" => new NanNode(),
            "char" => new CharNode(new UnicodePair(value)),
            "str" => new StringNode(value),
            "dec" => new DecimalNode(DecimalValue.Parse(value)),
            "uid" => new UidNode(Guid.Parse(value)),
            "col" => new ColorNode(ColorValue.Parse(value)),
            "bytes" => new BytesNode(BytesValue.Parse(value)),
            "memberSymbol" => new SymbolNode(value),
            "ref" => new RefNode(value),
            _ => throw new ArgumentException($"Invalid primitive tag <{tag}>.")
        };

        private static INode ReadNode(XElement element)
        {
            string tag = element.Name.LocalName;
            INode node;

            string addressName = (string)element.Attribute("id");
            string typeName = (string)element.Attribute("type");

            if (IsPrimitive(tag))
            {
                string value = element.Value;
                node = ParsePrimitive(tag, value);
            }
            else
            {
                node = tag switch
                {
                    "time" => ReadTime(element),
                    "dur" => ReadDuration(element),
                    "list" => ReadList(element),
                    "dict" => ReadDict(element),
                    "obj" => ReadObject(element),
                    _ => throw new ArgumentException($"Illegal XML tag <{tag}>.")
                };
            }

            if (!string.IsNullOrEmpty(typeName)) node = new TypeNode(typeName, node);
            if (!string.IsNullOrEmpty(addressName)) node = new AddressNode(addressName, node);

            return node;
        }

        private static ObjectNode ReadObject(XElement element)
        {
            ObjectNode obj = new ObjectNode();
            foreach (XElement member in element.Elements())
            {
                string fieldName = (string)member.Attribute("name");
                string scopeName = (string)member.Attribute("scope");
                IMemberNameNode memberName = null;
                if (!string.IsNullOrEmpty(scopeName))
                    memberName = new ScopeNode(scopeName, new SymbolNode(fieldName));
                else
                    memberName = new SymbolNode(fieldName);

                obj.AddMember(memberName, ReadNode(member));
            }
            return obj;
        }

        private static ListNode ReadList(XElement element)
        {
            ListNode list = new ListNode();
            foreach (XElement child in element.Elements())
            {
                list.AddValue(ReadNode(child));
            }
            return list;
        }

        private static DictNode ReadDict(XElement element)
        {
            DictNode dict = new DictNode();
            foreach (XElement entry in element.Elements("entry"))
            {
                XElement keyNode = entry.Element("key")?.Elements().FirstOrDefault();
                XElement valueNode = entry.Element("value")?.Elements().FirstOrDefault();
                if (keyNode == null || valueNode == null)
                    throw new ArgumentException("<entry> must contain <key> and <value> elements.");
                dict.AddPair(ReadNode(keyNode), ReadNode(valueNode));
            }
            return dict;
        }

        private static INode ReadTime(XElement element)
        {
            IntValue year = 1, month = 1, day = 1, hour = 0, minute = 0;
            FloatValue second = 0;

            foreach (XElement child in element.Elements())
            {
                string value = child.Value;
                switch (child.Name.LocalName)
                {
                    case "year": year = IntValue.Parse(value); break;
                    case "month": month = IntValue.Parse(value); break;
                    case "day": day = IntValue.Parse(value); break;
                    case "hour": hour = IntValue.Parse(value); break;
                    case "minute": minute = IntValue.Parse(value); break;
                    case "second": second = FloatValue.Parse(value); break;
                    default: throw new ArgumentException($"Invalid <time> member <{child.Name}>.");
                }
            }

            TimestampNode timestamp = new TimestampNode(year, month, day, hour, minute, second);

            string offset = (string)element.Attribute("offset");
            if (!string.IsNullOrEmpty(offset))
                return new OffsetNode(OffsetValue.Parse(offset), timestamp);
            return timestamp;
        }

        private static DurationNode ReadDuration(XElement element)
        {
            BoolValue negative = false;
            IntValue days = 0, hours = 0, minutes = 0;
            FloatValue seconds = 0;

            foreach (XElement child in element.Elements())
            {
                string value = child.Value;
                switch (child.Name.LocalName)
                {
                    case "negative": negative = true; break;
                    case "days": days = IntValue.Parse(value); break;
                    case "hours": hours = IntValue.Parse(value); break;
                    case "minutes": minutes = IntValue.Parse(value); break;
                    case "seconds": seconds = FloatValue.Parse(value); break;
                    default: throw new ArgumentException($"Invalid <time> member <{child.Name}>.");
                }
            }

            return new DurationNode(negative, days, hours, minutes, seconds);
        }
    }
}