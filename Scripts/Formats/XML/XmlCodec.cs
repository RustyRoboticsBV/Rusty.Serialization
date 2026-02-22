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

            // Unwrap TypeNode
            string typeName = null;
            if (node is TypeNode type)
            {
                typeName = type.Name;
                node = type.Value;
            }

            // Write node.
            switch (node)
            {
                case NullNode:
                    writer.WriteStartElement("null");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteEndElement();
                    break;

                case BoolNode b:
                    writer.WriteStartElement("bool");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(b.Value.ToString().ToLowerInvariant());
                    writer.WriteEndElement();
                    break;

                case IntNode i:
                    writer.WriteStartElement("int");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(i.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case FloatNode f:
                    writer.WriteStartElement("float");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(f.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case NanNode:
                    writer.WriteStartElement("nan");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteEndElement();
                    break;

                case InfinityNode inf:
                    writer.WriteStartElement("inf");
                    WriteMetadata(writer, addressName, typeName);
                    if (inf.Positive)
                        writer.WriteString("+");
                    else
                        writer.WriteString("-");

                    writer.WriteEndElement();
                    break;

                case CharNode chr:
                    writer.WriteStartElement("char");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(chr.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case StringNode s:
                    writer.WriteStartElement("str");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(s.Value);
                    writer.WriteEndElement();
                    break;

                case DecimalNode d:
                    writer.WriteStartElement("dec");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(d.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case ColorNode col:
                    writer.WriteStartElement("col");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(col.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case TimestampNode t:
                    writer.WriteStartElement("time");
                    WriteMetadata(writer, addressName, typeName);

                    if (t.Value.year != 1)
                        writer.WriteElementString("year", t.Value.year.ToString());
                    if (t.Value.month != 1)
                        writer.WriteElementString("month", t.Value.month.ToString());
                    if (t.Value.day != 1)
                        writer.WriteElementString("day", t.Value.day.ToString());
                    if (t.Value.hour != 0)
                        writer.WriteElementString("hour", t.Value.hour.ToString());
                    if (t.Value.minute != 0)
                        writer.WriteElementString("minute", t.Value.minute.ToString());
                    if (t.Value.second != 0)
                        writer.WriteElementString("second", t.Value.second.ToString());

                    writer.WriteEndElement();
                    break;

                case BytesNode by:
                    writer.WriteStartElement("bytes");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(by.Value.ToString());
                    writer.WriteEndElement();
                    break;

                case SymbolNode smb:
                    writer.WriteStartElement("symbol");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(smb.Name);
                    writer.WriteEndElement();
                    break;

                case RefNode r:
                    writer.WriteStartElement("ref");
                    WriteMetadata(writer, addressName, typeName);

                    writer.WriteString(r.Address);
                    writer.WriteEndElement();
                    break;

                case ListNode l:
                    writer.WriteStartElement("list");
                    WriteMetadata(writer, addressName, typeName);

                    foreach (var element in l.Elements)
                    {
                        WriteNode(writer, element);
                    }
                    writer.WriteEndElement();
                    break;

                case DictNode d:
                    writer.WriteStartElement("dict");
                    WriteMetadata(writer, addressName, typeName);

                    foreach (var pair in d.Pairs)
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

                case ObjectNode o:
                    writer.WriteStartElement("obj");
                    WriteMetadata(writer, addressName, typeName);

                    foreach (var member in o.Members)
                    {
                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("name", member.Key.ToString()); // TODO: fix
                        WriteNode(writer, member.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    break;

                default:
                    throw new NotSupportedException($"Unsupported node type {node.GetType().Name}");
            }
        }

        private static void WriteMetadata(XmlWriter writer, string address, string type)
        {
            if (address != null)
                writer.WriteAttributeString("id", address);
            if (type != null)
                writer.WriteAttributeString("type", type);
        }


        // Parsing.

        private static bool IsPrimitive(string tag) => tag is "null" or "bool" or "int" or "float" or "inf" or "nan" or "char"
            or "str" or "dec" or "col" or "bytes" or "symbol" or "ref";

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