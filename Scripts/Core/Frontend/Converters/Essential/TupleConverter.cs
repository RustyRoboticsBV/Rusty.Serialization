using System.Runtime.CompilerServices;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum flags converter.
    /// </summary>
    public class TupleConverter<T> : Converter
        where T : ITuple
    { }
}