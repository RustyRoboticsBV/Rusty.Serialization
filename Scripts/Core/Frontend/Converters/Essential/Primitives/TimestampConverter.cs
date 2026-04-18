using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic timestamp converter.
    /// </summary>
    public class TimestampConverter<T> : TypedConverter<TimestampValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new TimestampNode(ToTimestamp((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(TimestampNode node, CreateObjectContext context) => FromTimestamp(node.Value);

        /// <summary>
        /// Convert an timestamp object to the internal value representation.
        /// </summary>
        protected virtual TimestampValue ToTimestamp(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an timestamp object of the desired type.
        /// </summary>
        protected virtual T FromTimestamp(TimestampValue obj) => FromValue(obj);
    }
}