using Rusty.Serialization.Converters;
using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Serializers;
using Rusty.Serialization.Serializers;
using System;

namespace Rusty.Serialization
{
    /// <summary>
    /// A default serialization context. By default, it contains handlers for various .NET and Godot/Unity types, and serializes
    /// to the CSCD format.
    /// </summary>
    public class DefaultContext : Context
    {
        /* Public properties. */
        /// <summary>
        /// A set of default converters.
        /// </summary>
        public static DefaultConverters DefaultConverters => new();
        /// <summary>
        /// A CSCD serialization scheme.
        /// </summary>
        public static Serializers.CSCD.Scheme CscdScheme => new();
        /// <summary>
        /// A JSON serialization scheme.
        /// </summary>
        public static Serializers.JSON.Scheme JsonScheme => new();
        /// <summary>
        /// An XML serialization scheme.
        /// </summary>
        public static Serializers.XML.Scheme XmlScheme => new();

        public override IConverterScheme ConverterScheme { get; set; } = DefaultConverters;
        public override ISerializerScheme SerializerScheme { get; set; } = CscdScheme;

        /* Constructors. */
        public DefaultContext(Format format = Format.Cscd)
        {
            switch (format)
            {
                case Format.Cscd:
                    SerializerScheme = CscdScheme;
                    break;
                case Format.Json:
                    SerializerScheme = JsonScheme;
                    break;
                case Format.Xml:
                    SerializerScheme = XmlScheme;
                    break;
            }
        }
    }
}