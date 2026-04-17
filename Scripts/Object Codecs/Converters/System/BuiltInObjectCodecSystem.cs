using System;
using Rusty.Serialization.Conversion.System;

namespace Rusty.Serialization.Conversion
{
    public partial class BuiltInObjectCodec
    {
        /* Private methods. */
        private void AddSystemTypes()
        {
            Converters.Add<bool, BoolConverter>();

            Converters.Add<sbyte, SbyteConverter>();
            Converters.Add<short, ShortConverter>();
            Converters.Add<int, IntConverter>();
            Converters.Add<long, LongConverter>();
            Converters.Add<byte, ByteConverter>();
            Converters.Add<ushort, UshortConverter>();
            Converters.Add<uint, UintConverter>();
            Converters.Add<ulong, UlongConverter>();

            Converters.Add<float, FloatConverter>();
            Converters.Add<double, DoubleConverter>();

            Converters.Add<decimal, DecimalConverter>();

            Converters.Add<char, CharConverter>();

            Converters.Add<string, StringConverter>();

            Converters.Add<Guid, GuidConverter>();

            Converters.Add<DateTime, DateTimeConverter>();

            Converters.Add<TimeSpan, TimeSpanConverter>();
        }
    }
}