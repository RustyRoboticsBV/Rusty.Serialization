using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A nullable type converter.
    /// </summary>
    public sealed class NullableConverter<T> : IConverter
        where T : struct
    {
        /* Public methods. */
        public INode CreateNode(object obj)
        {
            return null; // TODO: IMPLEMENT NULLABLE CONVERTER.
        }

        public object CreateObject(INode node, CreateObjectContext context)
        {
            return null; // TODO: IMPLEMENT NULLABLE CONVERTER.
        }
    }
}