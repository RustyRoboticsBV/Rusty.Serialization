using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converters that convert between a specific type and an bytes node.
    /// </summary>
    public abstract class TypedBytesConverter<T> : TypedConverter<T, BytesNode>
    {
        /* Public methods. */
        protected sealed override BytesNode CreateNode2(T obj, CreateNodeContext context) => new BytesNode(ToBytes(obj));
        protected sealed override T CreateObject2(BytesNode node, CreateObjectContext context) => FromBytes(node.Value);

        /* Protected methods. */
        /// <summary>
        /// Get the node value representation of the object.
        /// </summary>
        protected abstract BytesValue ToBytes(T obj);

        /// <summary>
        /// Parse a node value into an object.
        /// </summary>
        protected abstract T FromBytes(BytesValue value);
    }
}