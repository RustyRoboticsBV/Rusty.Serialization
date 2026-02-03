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

using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.DotNet;

namespace Rusty.Serialization
{
    public partial class DefaultConverters : Converters
    {
        /// <summary>
        /// Add the .NET type converters.
        /// </summary>
        private void AddSystem()
        {
            // System.
#if NETCOREAPP2_0_OR_GREATER
            ConverterRegistry.Add<DBNull, DBNullConverter>();
#endif

#if NETCOREAPP3_0_OR_GREATER
            ConverterRegistry.Add<Index, IndexConverter>();
#endif

#if NET5_0_OR_GREATER
            ConverterRegistry.Add<Half, HalfConverter>();
#endif

            ConverterRegistry.Add<Uri, UriConverter>();
            ConverterRegistry.Add<Version, VersionConverter>();
            ConverterRegistry.Add<Type, TypeConverter>();

            ConverterRegistry.Add<TimeSpan, TimeSpanConverter>();
            ConverterRegistry.Add<DateTime, DateTimeConverter>();
#if NET6_0_OR_GREATER
            ConverterRegistry.Add<DateOnly, DateOnlyConverter>();
            ConverterRegistry.Add<TimeOnly, TimeOnlyConverter>();
#endif

            ConverterRegistry.Add<Guid, GuidConverter>();

#if NETCOREAPP3_0_OR_GREATER
            ConverterRegistry.Add<Range, RangeConverter>();
#endif
            ConverterRegistry.Add<DateTimeOffset, DateTimeOffsetConverter>();

            // System.Numerics.
            ConverterRegistry.Add<BigInteger, BigIntegerConverter>();

            ConverterRegistry.Add<Complex, ComplexConverter>();
            ConverterRegistry.Add<Vector2, Vector2Converter>();
            ConverterRegistry.Add<Vector3, Vector3Converter>();
            ConverterRegistry.Add<Vector4, Vector4Converter>();
            ConverterRegistry.Add<Quaternion, QuaternionConverter>();
            ConverterRegistry.Add<Plane, PlaneConverter>();
            ConverterRegistry.Add<Matrix3x2, Matrix3x2Converter>();
            ConverterRegistry.Add<Matrix4x4, Matrix4x4Converter>();

            // System.Text.
#if NET5_0_OR_GREATER
            ConverterRegistry.Add<Rune, RuneConverter>();
#endif

            ConverterRegistry.Add<StringBuilder, StringBuilderConverter>();
            ConverterRegistry.Add<Encoding, EncodingConverter>();

            // System.Drawing.
#if NETCOREAPP2_0_OR_GREATER
            ConverterRegistry.Add<Color, ColorConverter>();

            ConverterRegistry.Add<Point, PointConverter>();
            ConverterRegistry.Add<PointF, PointFConverter>();
            ConverterRegistry.Add<Size, SizeConverter>();
            ConverterRegistry.Add<SizeF, SizeFConverter>();
            ConverterRegistry.Add<Rectangle, RectangleConverter>();
            ConverterRegistry.Add<RectangleF, RectangleFConverter>();
#endif

            // System.Collections
            ConverterRegistry.Add<BitArray, BitArrayConverter>();

            // System.Collections.Generic.
            ConverterRegistry.Add(typeof(List<>), typeof(ListConverter<>));
            ConverterRegistry.Add(typeof(LinkedList<>), typeof(LinkedListConverter<>));
            ConverterRegistry.Add(typeof(Stack<>), typeof(StackConverter<>));
            ConverterRegistry.Add(typeof(Queue<>), typeof(QueueConverter<>));
            ConverterRegistry.Add(typeof(HashSet<>), typeof(HashSetConverter<>));
            ConverterRegistry.Add(typeof(SortedSet<>), typeof(SortedSetConverter<>));
            ConverterRegistry.Add(typeof(SortedList<,>), typeof(SortedListConverter<,>));
            ConverterRegistry.Add(typeof(SortedDictionary<,>), typeof(SortedDictionaryConverter<,>));

            ConverterRegistry.Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>));
            ConverterRegistry.Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>));
#if NET6_0_OR_GREATER
            ConverterRegistry.Add(typeof(PriorityQueue<,>), typeof(PriorityQueueConverter<,>));
#endif
        }
    }
}
