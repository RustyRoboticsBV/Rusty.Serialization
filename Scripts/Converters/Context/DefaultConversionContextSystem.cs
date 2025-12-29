using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.DotNet;
using ColorConverter = Rusty.Serialization.DotNet.ColorConverter;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        private void AddSystem()
        {
            // System.
            Converters.Add<DBNull, DBNullConverter>();

#if NETCOREAPP3_0_OR_GREATER
            Converters.Add<Index, IndexConverter>();
#endif

#if NET5_0_OR_GREATER
            Converters.Add<Half, HalfConverter>();
#endif

            Converters.Add<Uri, UriConverter>();
            Converters.Add<Version, VersionConverter>();
            Converters.Add<Type, TypeConverter>();

            Converters.Add<TimeSpan, TimeSpanConverter>();
            Converters.Add<DateTime, DateTimeConverter>();
#if NET6_0_OR_GREATER
            Converters.Add<DateOnly, DateOnlyConverter>();
            Converters.Add<TimeOnly, TimeOnlyConverter>();
#endif

            Converters.Add<Guid, GuidConverter>();

#if NETCOREAPP3_0_OR_GREATER
            Converters.Add<Range, RangeConverter>();
#endif
            Converters.Add<DateTimeOffset, DateTimeOffsetConverter>();

            // System.Collections
            Converters.Add<BitArray, BitArrayConverter>();

            // System.Collections.Generic.
            Converters.Add(typeof(List<>), typeof(ListConverter<>));

            Converters.Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>));

            // System.Drawing.
            Converters.Add<Color, ColorConverter>();

            // System.Text.
#if NET5_0_OR_GREATER
            Converters.Add<Rune, RuneConverter>();
#endif

            Converters.Add<StringBuilder, StringBuilderConverter>();
            Converters.Add<Encoding, EncodingConverter>();
        }
    }
}
