using System;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum flags converter.
    /// </summary>
    public class FlagsConverter<T> : Converter
        where T : Enum
    { }
}