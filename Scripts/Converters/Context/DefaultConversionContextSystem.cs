using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

#if NETCOREAPP2_0_OR_GREATER
using System.Drawing;
using ColorConverter = Rusty.Serialization.DotNet.ColorConverter;
using PointConverter = Rusty.Serialization.DotNet.PointConverter;
using SizeConverter = Rusty.Serialization.DotNet.SizeConverter;
using SizeFConverter = Rusty.Serialization.DotNet.SizeFConverter;
using RectangleConverter = Rusty.Serialization.DotNet.RectangleConverter;
#endif

using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.DotNet;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        private void AddSystem()
        {
            // System.
#if NETCOREAPP2_0_OR_GREATER
            Converters.Add<DBNull, DBNullConverter>();
#endif

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

            // System.Numerics.
            Converters.Add<BigInteger, BigIntegerConverter>();

            Converters.Add<Complex, ComplexConverter>();
            Converters.Add<Vector2, Vector2Converter>();
            Converters.Add<Vector3, Vector3Converter>();
            Converters.Add<Vector4, Vector4Converter>();
            Converters.Add<Quaternion, QuaternionConverter>();
            Converters.Add<Plane, PlaneConverter>();
            Converters.Add<Matrix3x2, Matrix3x2Converter>();
            Converters.Add<Matrix4x4, Matrix4x4Converter>();

            // System.Text.
#if NET5_0_OR_GREATER
            Converters.Add<Rune, RuneConverter>();
#endif

            Converters.Add<StringBuilder, StringBuilderConverter>();
            Converters.Add<Encoding, EncodingConverter>();

            // System.Drawing.
#if NETCOREAPP2_0_OR_GREATER
            Converters.Add<Color, ColorConverter>();

            Converters.Add<Point, PointConverter>();
            Converters.Add<PointF, PointFConverter>();
            Converters.Add<Size, SizeConverter>();
            Converters.Add<SizeF, SizeFConverter>();
            Converters.Add<Rectangle, RectangleConverter>();
            Converters.Add<RectangleF, RectangleFConverter>();
#endif

            // System.Collections
            Converters.Add<BitArray, BitArrayConverter>();

            // System.Collections.Generic.
            Converters.Add(typeof(List<>), typeof(ListConverter<>));

            Converters.Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>));
        }
    }
}
