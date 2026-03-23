using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converters that convert between a specific type and a UID node.
    /// </summary>
    public abstract class TypedUidConverter<T> : TypedConverter<T, UidNode>
    {
        /* Public methods. */
        protected sealed override UidNode CreateNode2(T obj, CreateNodeContext context) => new UidNode(ToUid(obj));
        protected sealed override T CreateObject2(UidNode node, CreateObjectContext context) => FromUid(node.Value);

        /* Protected methods. */
        /// <summary>
        /// Get the node value representation of the object.
        /// </summary>
        protected abstract UidValue ToUid(T obj);

        /// <summary>
        /// Parse a node value into an object.
        /// </summary>
        protected abstract T FromUid(UidValue value);
    }
}