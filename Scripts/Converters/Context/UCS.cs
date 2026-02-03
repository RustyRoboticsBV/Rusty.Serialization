using System;

using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Codecs;

using Rusty.Serialization.CSCD;
using Rusty.Serialization.JSON;
using Rusty.Serialization.XML;

namespace Rusty.Serialization
{
    /// <summary>
    /// The Universal C# Serializer main utility class.
    /// It can serialize objects to text and deserialize text back to an object, using an object-to-node tree front-end and a 
    /// node tree-to-format back-end.
    /// </summary>
    public static class UCS
    {
        /* Fields. */
        public static DefaultConverters DefaultConverters = new DefaultConverters();

        public static CscdCodec Cscd = new CscdCodec();
        public static JsonCodec Json = new JsonCodec();
        public static XmlCodec Xml = new XmlCodec();

        /* Public methods. */
        /// <summary>
        /// Serialize an object to a string.
        /// </summary>
        public static string Serialize<T>(T obj, Format format = Format.Cscd, bool prettyPrint = true)
        {
            return Serialize(obj, DefaultConverters, format, prettyPrint);
        }

        /// <summary>
        /// Serialize an object to a string.
        /// </summary>
        public static string Serialize<T>(T obj, Converters converters, Format format = Format.Cscd, bool prettyPrint = true)
        {
            switch (format)
            {
                case Format.Cscd: return Serialize(obj, converters, Cscd, prettyPrint);
                case Format.Json: return Serialize(obj, converters, Json, prettyPrint);
                case Format.Xml: return Serialize(obj, converters, Xml, prettyPrint);
                default: throw new NotImplementedException(format.ToString());
            }
        }

        /// <summary>
        /// Serialize an object to a string.
        /// </summary>
        public static string Serialize<T>(T obj, Converters converters, Codec codec, bool prettyPrint = true)
        {
            NodeTree tree = ConvertObject(obj, converters);
            return SerializeTree(tree, codec, prettyPrint);
        }

        /// <summary>
        /// Serialize an object to a string.
        /// </summary>
        public static string Serialize(object obj, Type type, Converters converters, Codec codec, bool prettyPrint = true)
        {
            NodeTree tree = ConvertObject(obj, type, converters);
            return SerializeTree(tree, codec, prettyPrint);
        }


        /// <summary>
        /// Deserialize a string into an object.
        /// </summary>
        public static T Parse<T>(string serialized, Format format = Format.Cscd)
        {
            return Parse<T>(serialized, DefaultConverters, format);
        }

        /// <summary>
        /// Deserialize a string into an object.
        /// </summary>
        public static T Parse<T>(string serialized, Converters converters, Format format = Format.Cscd)
        {
            switch (format)
            {
                case Format.Cscd: return Parse<T>(serialized, converters, Cscd);
                case Format.Json: return Parse<T>(serialized, converters, Json);
                case Format.Xml: return Parse<T>(serialized, converters, Xml);
                default: throw new NotImplementedException(format.ToString());
            }
        }

        /// <summary>
        /// Deserialize a string into an object.
        /// </summary>
        public static T Parse<T>(string serialized, Converters converters, Codec codec)
        {
            NodeTree tree = ParseText(serialized, codec);
#if UNITY_5_3_OR_NEWER
            UnityEngine.Debug.Log(tree);
#endif
            return DeconvertTree<T>(tree, converters);
        }

        /// <summary>
        /// Deserialize a string into an object.
        /// </summary>
        public static object Parse(string serialized, Type type, Converters converters, Codec codec)
        {
            NodeTree tree = ParseText(serialized, codec);
#if UNITY_5_3_OR_NEWER
            UnityEngine.Debug.Log(tree);
#endif
            return DeconvertTree(type, tree, converters);
        }


        /// <summary>
        /// Convert serialized text from one format to another.
        /// </summary>
        public static string Reformat(string serialized, Format sourceFormat, Format targetFormat, bool prettyPrint = true)
        {
            Codec sourceCodec;
            switch (sourceFormat)
            {
                case Format.Cscd: sourceCodec = Cscd; break;
                case Format.Json: sourceCodec = Json; break;
                case Format.Xml: sourceCodec = Xml; break;
                default: throw new NotImplementedException(sourceFormat.ToString());
            }

            Codec targetCodec;
            switch (targetFormat)
            {
                case Format.Cscd: targetCodec = Cscd; break;
                case Format.Json: targetCodec = Json; break;
                case Format.Xml: targetCodec = Xml; break;
                default: throw new NotImplementedException(targetFormat.ToString());
            }

            NodeTree tree = ParseText(serialized, sourceCodec);
            return SerializeTree(tree, targetCodec, prettyPrint);
        }

        /// <summary>
        /// Convert serialized text from one format to another.
        /// </summary>
        public static string Reformat(string serialized, Codec sourceCodec, Codec targetCodec, bool prettyPrint = true)
        {
            NodeTree tree = sourceCodec.Parse(serialized);
            return targetCodec.Serialize(tree, prettyPrint);
        }

        /* Private methods. */

        // Conversion.

        /// <summary>
        /// Serialize an object to a node tree.
        /// </summary>
        private static NodeTree ConvertObject<T>(T obj, Converters converters)
        {
            if (converters == null)
                throw new ArgumentNullException(nameof(converters));
            return converters.Convert(obj);
        }

        /// <summary>
        /// Convert an object to a node tree.
        /// </summary>
        private static NodeTree ConvertObject(object obj, Type type, Converters converters)
        {
            if (converters == null)
                throw new ArgumentNullException(nameof(converters));
            return converters.Convert(obj, type);
        }

        // Deconversion.

        /// <summary>
        /// Deserialize a node tree to an object.
        /// </summary>
        private static T DeconvertTree<T>(NodeTree tree, Converters converters)
        {
            if (converters == null)
                throw new ArgumentNullException(nameof(converters));
            return converters.Deconvert<T>(tree);
        }

        /// <summary>
        /// Deserialize a node tree to an object.
        /// </summary>
        private static object DeconvertTree(Type type, NodeTree tree, Converters converters)
        {
            if (converters == null)
                throw new ArgumentNullException(nameof(converters));
            return converters.Deconvert(type, tree);
        }

        // Serialization.

        /// <summary>
        /// Serialize a node tree to text.
        /// </summary>
        private static string SerializeTree(NodeTree tree, Codec codec, bool prettyPrint)
        {
            if (codec == null)
                throw new ArgumentNullException(nameof(codec));
            return codec.Serialize(tree, prettyPrint);
        }

        // Parsing.

        /// <summary>
        /// Deserialize text to a node tree.
        /// </summary>
        private static NodeTree ParseText(string serialized, Codec codec)
        {
            if (codec == null)
                throw new ArgumentNullException(nameof(codec));
            return codec.Parse(serialized);
        }
    }
}