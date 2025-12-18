using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Dotnet;
using ColorConverter = Rusty.Serialization.Dotnet.ColorConverter;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        /* Constructors. */
        public void AddDotnetConverters()
        {
            ConverterTypes.Add<Color, ColorConverter>();
            ConverterTypes.Add<DateTime, DateTimeConverter>();

            ConverterTypes.Add(typeof(List<>), typeof(ListConverter<>));
            //ConverterTypes.Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>));

            ConverterTypes.Add<Rune, RuneConverter>();
        }
    }
}