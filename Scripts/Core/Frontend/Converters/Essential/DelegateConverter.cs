using System;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A delegate converter.
    /// </summary>
    public class DelegateConverter<T> : Converter
        where T : Delegate
    { }
}