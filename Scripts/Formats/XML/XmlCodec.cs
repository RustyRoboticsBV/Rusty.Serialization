using System;
using System.Xml;
using System.IO;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.XML
{
    /// <summary>
    /// A XML serializer/deserializer back-end.
    /// </summary>
    public class XmlCodec : Codec
    {
        /* Public methods. */
        public override string Serialize(NodeTree node, bool prettyPrint)
        {
            throw new NotImplementedException();
        }

        public override NodeTree Parse(string serialized)
        {
            return new NodeTree(ParseXml(serialized));
        }

        private INode ParseXml(string serialized)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                DtdProcessing = DtdProcessing.Ignore
            };

            StringReader strReader = new StringReader(serialized);
            using XmlReader xmlReader = XmlReader.Create(strReader, settings);

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType != XmlNodeType.Element)
                    continue;

                string str = xmlReader.ReadContentAsString();

                if (xmlReader.Name == "null")
                    return new NullNode();
                if (xmlReader.Name == "bool")
                    return new BoolNode(bool.Parse(str));
                if (xmlReader.Name == "int")
                    return new IntNode(IntValue.Parse(str));
                if (xmlReader.Name == "float")
                    return new FloatNode(FloatValue.Parse(str));
                if (xmlReader.Name == "inf")
                {
                    if (str == "+")
                        return new InfinityNode(true);
                    if (str == "-")
                        return new InfinityNode(false);
                }
                if (xmlReader.Name == "nan")
                    return new NanNode();
                if (xmlReader.Name == "char")
                    return new CharNode(new UnicodePair(str));
                if (xmlReader.Name == "str")
                    return new StringNode(str);
                if (xmlReader.Name == "dec")
                    return new DecimalNode(DecimalValue.Parse(str));
                if (xmlReader.Name == "col")
                {
                    throw new NotImplementedException();
                }
                if (xmlReader.Name == "time")
                {
                    throw new NotImplementedException();
                }
                if (xmlReader.Name == "bytes")
                {
                    throw new NotImplementedException();
                }
                if (xmlReader.Name == "list")
                {
                    throw new NotImplementedException();
                }
                if (xmlReader.Name == "dict")
                {
                    throw new NotImplementedException();
                }
                if (xmlReader.Name == "obj")
                {
                    throw new NotImplementedException();
                }
                break;
            }
            throw new ArgumentException($"Illegal XML tag {xmlReader.Name}.");
        }
    }
}