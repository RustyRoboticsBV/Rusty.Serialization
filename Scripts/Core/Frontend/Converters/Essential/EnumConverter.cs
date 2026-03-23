using System;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum converter.
    /// </summary>
    public class EnumConverter<T> : Converter
        where T : Enum
    { }
}