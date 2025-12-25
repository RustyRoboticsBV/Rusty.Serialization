using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization
{
    public class DefaultConversionContext : ConversionContext
    {
        public DefaultConversionContext() : base()
        {
            Converters.Add(typeof(Dictionary<,>), typeof(DotNet.DictionaryConverter<,>));
        }
    }
}
