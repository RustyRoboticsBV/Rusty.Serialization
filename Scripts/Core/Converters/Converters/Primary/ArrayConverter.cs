namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An array converter.
    /// </summary>
    public sealed class ArrayConverter<T> : ListConverter<T[], T>
    {
        /* Protected methods. */
        protected override T[] CreateObjectFromElements(T[] elements) => elements;
    }
}