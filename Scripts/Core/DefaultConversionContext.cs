using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.DotNet;

namespace Rusty.Serialization
{
    public class DefaultConversionContext : ConversionContext
    {
        public DefaultConversionContext() : base()
        {
            Converters.Add(typeof(List<>), typeof(ListConverter<>));
            Converters.Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>));
        }
    }
}
