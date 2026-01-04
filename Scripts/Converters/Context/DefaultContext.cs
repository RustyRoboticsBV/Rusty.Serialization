using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization
{
    /// <summary>
    /// The default serialization context.
    /// </summary>
    public class DefaultContext
    {
        /* Public properties. */
        /// <summary>
        /// The CSCD serializers.
        /// </summary>
        public static CSCD.Scheme Cscd => new CSCD.Scheme();

        /// <summary>
        /// The converter scheme.
        /// </summary>
        public ConversionContext Converter { get; set; }
        /// <summary>
        /// The serializer scheme.
        /// </summary>
        public ISerializerScheme Serializer { get; set; }

        /* Constructors. */
        public DefaultContext(ConversionContext converter, ISerializerScheme serializer)
        {
            Converter = converter;
            Serializer = serializer;
        }

        public DefaultContext(Format format)
        {
            switch (format)
            {
                case Format.Cscd:
                    Serializer = Cscd;
                    break;
                case Format.Json:
                case Format.Xml:
                default:
                    throw new NotImplementedException(format.ToString());
            }

            Converter = new DefaultConversionContext();
        }

        /* Public methods. */
        /// <summary>
        /// Serialize an object.
        /// </summary>
        public string Serialize<T>(T obj, bool prettyPrint = false)
        {
            if (Converter == null)
                throw new NullReferenceException(nameof(Converter));
            if (Serializer == null)
                throw new NullReferenceException(nameof(Serializer));

            NodeTree tree = Converter.Convert(obj);
            return Serializer.Serialize(tree, prettyPrint);
        }

        /// <summary>
        /// Deserialize an object.
        /// </summary>
        public T Parse<T>(string serialized)
        {
            NodeTree tree = Serializer.ParseAsTree(serialized);
            return Converter.Deconvert<T>(tree);
        }
    }
}