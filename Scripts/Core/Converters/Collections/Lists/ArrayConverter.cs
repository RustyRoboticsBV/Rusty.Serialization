namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic array converter.
    /// </summary>
    public sealed class ArrayConverter<T> : GenericListConverter<T[], T>
    {
        /* Protected methods. */
        protected override T[] CreateObject(T[] elements) => elements;
    }
}