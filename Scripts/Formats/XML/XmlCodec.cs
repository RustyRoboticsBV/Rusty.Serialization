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
            return new NodeTree(rootNode);
        }

        /* Private methods. */

        // Writing.

        private static void WriteNode(XmlWriter writer, INode node)
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
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteEndElement();
                    break;

                case BoolNode @bool:
                    writer.WriteStartElement("bool");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(@bool.Value.ToString().ToLowerInvariant());
                    writer.WriteEndElement();
                    break;

                case IntNode @int:
                    writer.WriteStartElement("int");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(@int.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case FloatNode @float:
                    writer.WriteStartElement("float");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(@float.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case NanNode:
                    writer.WriteStartElement("nan");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteEndElement();
                    break;

                case InfinityNode inf:
                    writer.WriteStartElement("inf");
                    WriteMetadata(writer, addressName, typeName, offsetValue);
                    if (inf.Positive)
                        writer.WriteString("+");
                    else
                        writer.WriteString("-");

                    writer.WriteEndElement();
                    break;

                case CharNode chr:
                    writer.WriteStartElement("char");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(chr.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case StringNode str:
                    writer.WriteStartElement("str");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(str.Value);
                    writer.WriteEndElement();
                    break;

                case DecimalNode @decimal:
                    writer.WriteStartElement("dec");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(@decimal.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case ColorNode color:
                    writer.WriteStartElement("col");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(color.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case UidNode uid:
                    writer.WriteStartElement("uid");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(uid.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case TimestampNode timestamp:
                    writer.WriteStartElement("time");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

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
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(duration.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case BytesNode bytes:
                    writer.WriteStartElement("bytes");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(bytes.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case SymbolNode symbol:
                    writer.WriteStartElement("symbol");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(symbol.Name);
                    writer.WriteEndElement();
                    break;

                case RefNode @ref:
                    writer.WriteStartElement("ref");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    writer.WriteString(@ref.Address);
                    writer.WriteEndElement();
                    break;

                case ListNode list:
                    writer.WriteStartElement("list");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    foreach (var element in list.Elements)
                    {
                        WriteNode(writer, element);
                    }
                    writer.WriteEndElement();
                    break;

                case DictNode dict:
                    writer.WriteStartElement("dict");
                    WriteMetadata(writer, addressName, typeName, offsetValue);

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
                    WriteMetadata(writer, addressName, typeName, offsetValue);

                    foreach (var member in obj.Members)
                    {
                        writer.WriteStartElement("field");
                        if (member.Key is ScopeNode scope)
                        {
                            writer.WriteAttributeString("scope", scope.Name);
                            writer.WriteAttributeString("name", scope.Value.Name);
                        }
                        else if (member.Key is SymbolNode symbol)
                            writer.WriteAttributeString("name", symbol.Name);
                        WriteNode(writer, member.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    break;

                default:
                    throw new NotSupportedException($"Unsupported node type {node.GetType().Name}");
            }
        }

        private static void WriteMetadata(XmlWriter writer, string address, string type, string offset)
            => WriteMetadata(writer, address, type, null, offset);

        private static void WriteMetadata(XmlWriter writer, string scope)
            => WriteMetadata(writer, null, null, scope, null);

        private static void WriteMetadata(XmlWriter writer, string address, string type, string scope, string offset)
        {
            if (address != null)
                writer.WriteAttributeString("id", address);
            if (type != null)
                writer.WriteAttributeString("type", type);
            if (scope != null)
                writer.WriteAttributeString("scope", scope);
            if (offset != null)
                writer.WriteAttributeString("offset", offset);
        }


        // Parsing.

        private static bool IsPrimitive(string tag) => tag is "null" or "bool" or "int" or "float" or "inf" or "nan" or "char"
            or "str" or "dec" or "col" or "uid" or "bytes" or "symbol" or "ref";

        private static INode ParsePrimitive(string tag, string value) => tag switch
        {
            "null" => new NullNode(),
            "bool" => new BoolNode(bool.Parse(value)),
            "int" => new IntNode(IntValue.Parse(value)),
            "float" => new FloatNode(FloatValue.Parse(value)),
            "inf" => value == "+" ? new InfinityNode(true) : new InfinityNode(false),
            "nan" => new NanNode(),
            "char" => new CharNode(new UnicodePair(value)),
            "str" => new StringNode(value),
            "dec" => new DecimalNode(DecimalValue.Parse(value)),
            "col" => new ColorNode(ColorValue.Parse(value)),
            "bytes" => new BytesNode(BytesValue.Parse(value)),
            "symbol" => new SymbolNode(value),
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
            foreach (XElement field in element.Elements("field"))
            {
                string fieldName = (string)field.Attribute("name");
                XElement valueNode = field.Elements().FirstOrDefault();
                if (valueNode == null) throw new ArgumentException($"Field {fieldName} has no value element.");
                obj.AddMember(/*fieldName*/null, ReadNode(valueNode)); // TODO: fix.
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

        private static TimestampNode ReadTime(XElement element)
        {
            IntValue year = 1;
            byte month = 1, day = 1, hour = 0, minute = 0;
            FloatValue second = 0;

            foreach (XElement child in element.Elements())
            {
                string value = child.Value;
                switch (child.Name.LocalName)
                {
                    case "year": year = IntValue.Parse(value); break;
                    case "month": month = byte.Parse(value); break;
                    case "day": day = byte.Parse(value); break;
                    case "hour": hour = byte.Parse(value); break;
                    case "minute": minute = byte.Parse(value); break;
                    case "second": second = FloatValue.Parse(value); break;
                    default: throw new ArgumentException($"Invalid <time> field <{child.Name}>.");
                }
            }

            return new TimestampNode(year, month, day, hour, minute, second);
        }
    }
}