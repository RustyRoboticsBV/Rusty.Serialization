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
    public sealed class UCS
    {
        /* Fields. */
        public static DefaultConverters DefaultConverters = new DefaultConverters();

        public static CscdCodec Cscd = new CscdCodec();
        public static JsonCodec Json = new JsonCodec();
        public static XmlCodec Xml = new XmlCodec();

        /* Public properties. */
        public Converters Converters { get; set; }
        public Codec Codec { get; set; }

        /* Constructors. */
        public UCS(Converters converters, Format format)
        {
            Converters = converters;
            switch (format)
            {
                case Format.Cscd:
                    Codec = Cscd;
                    break;
                case Format.Json:
                    Codec = Json;
                    break;
                case Format.Xml:
                    Codec = Xml;
                    break;
            }
        }

        public UCS(Converters converters, Codec format)
        {
            Converters = converters;
            Codec = format;
        }

        /* Public methods. */
        /// <summary>
        /// Serialize an object to a string.
        /// </summary>
        public string Serialize<T>(T obj, Settings settings = Settings.IncludeFormatHeader)
        {
            if (Codec == null)
                throw new InvalidOperationException("No codec format specified.");

            NodeTree tree = Converters.Convert(obj);
            return Codec.Serialize(tree, settings);
        }

        /// <summary>
        /// Serialize an object to a string.
        /// </summary>
        public string Serialize(object obj, Settings settings = Settings.IncludeFormatHeader)
        {
            if (Codec == null)
                throw new InvalidOperationException("No codec format specified.");

            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            NodeTree tree = Converters.Convert(obj, obj.GetType());
            return Codec.Serialize(tree, settings);
        }


        /// <summary>
        /// Deserialize a string into an object.
        /// </summary>
        public T Parse<T>(string serialized)
        {
            if (Codec == null)
                throw new InvalidOperationException("No codec format specified.");

            NodeTree tree = Codec.Parse(serialized);
            return Converters.Deconvert<T>(tree);
        }

        /// <summary>
        /// Deserialize a string into an object.
        /// </summary>
        public object Parse(string serialized)
        {
            if (Codec == null)
                throw new InvalidOperationException("No codec format specified.");

            NodeTree tree = Codec.Parse(serialized);
            return Converters.Deconvert(tree);
        }


        /// <summary>
        /// Reformat a string from this format to another.
        /// </summary>
        public string ReformatTo(string serialized, UCS targetFormat, Settings settings = Settings.IncludeFormatHeader)
        {
            if (targetFormat == null)
                throw new ArgumentNullException(nameof(targetFormat));

            return ReformatTo(serialized, targetFormat.Codec);
        }

        /// <summary>
        /// Reformat a string from this format to another.
        /// </summary>
        public string ReformatTo(string serialized, Codec targetFormat, Settings settings = Settings.IncludeFormatHeader)
        {
            if (Codec == null)
                throw new InvalidOperationException("No source codec format specified.");

            if (targetFormat == null)
                throw new ArgumentNullException(nameof(targetFormat));

            NodeTree tree = Codec.Parse(serialized);
            return targetFormat.Serialize(tree, settings);
        }
    }
}