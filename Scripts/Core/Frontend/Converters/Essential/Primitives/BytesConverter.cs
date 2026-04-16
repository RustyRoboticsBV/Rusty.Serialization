using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic bytes converter.
    /// </summary>
    public class BytesConverter<T> : TypedConverter<BytesValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new BytesNode(ToBytes((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(BytesNode node, CreateObjectContext context) => FromBytes(node.Value);

        protected sealed override object PopulateObject(BytesNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an bytes object to the internal value representation.
        /// </summary>
        protected virtual BytesValue ToBytes(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an bytes object of the desired type.
        /// </summary>
        protected virtual T FromBytes(BytesValue obj) => FromValue(obj);
    }
}