namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic struct converter.
    /// </summary>
    public sealed class StructConverter<T> : ObjectConverter<T>
        where T : struct
    { }
}