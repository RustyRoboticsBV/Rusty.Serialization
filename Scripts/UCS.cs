using System;
using Rusty.Serialization.Conversion;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.CSCD;
using Rusty.Serialization.CSV;
using Rusty.Serialization.JSON;
using Rusty.Serialization.XML;

namespace Rusty.Serialization
{
    public class UCS
    {
        /* Public properties. */
        public ObjectCodec ObjectCodec { get; set; }
        public FormatCodec FormatCodec { get; set; }

        /* Constructors. */
        public UCS(ObjectCodec objectCodec, FormatCodec formatCodec)
        {
            ObjectCodec = objectCodec;
            FormatCodec = formatCodec;
        }

        public UCS(FormatCodec formatCodec) : this(new BuiltInObjectCodec(), formatCodec) { }

        public UCS(ObjectCodec objectCodec, Format format) : this(objectCodec, CreateFormatCodec(format)) { }

        public UCS(Format format) : this(new BuiltInObjectCodec(), format) { }

        /* Public methods. */
        public string Serialize(object obj) => FormatCodec.Serialize(ObjectCodec.Convert(obj));

        /* Private methods. */
        private static FormatCodec CreateFormatCodec(Format format)
        {
            switch (format)
            {
                case Format.Cscd:
                    return new CscdCodec();
                case Format.Json:
                    return new JsonCodec();
                case Format.Xml:
                    return new XmlCodec();
                case Format.Csv:
                    return new CsvCodec();
                default:
                    throw new ArgumentException("Invalid format.", nameof(format));
            }
        }
    }
}