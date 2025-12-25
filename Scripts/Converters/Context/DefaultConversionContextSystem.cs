using System;
using System.Collections.Generic;
using System.Drawing;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.DotNet;
using ColorConverter = Rusty.Serialization.DotNet.ColorConverter;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        private void AddSystem()
        {
            Converters.Add<Color, ColorConverter>();
            Converters.Add<DateTime, DateTimeConverter>();
            Converters.Add(typeof(List<>), typeof(ListConverter<>));
            Converters.Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>));
        }
    }
}
