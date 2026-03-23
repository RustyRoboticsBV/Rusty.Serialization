using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converters that convert between a specific type and an bitmask node.
    /// </summary>
    public abstract class TypedBitmaskConverter<T> : TypedConverter<T, BitmaskNode>
    {
        /* Public methods. */
        protected sealed override BitmaskNode CreateNode2(T obj, CreateNodeContext context) => new BitmaskNode(ToBitmask(obj));
        protected sealed override T CreateObject2(BitmaskNode node, CreateObjectContext context) => FromBitmask(node.Value);

        /* Protected methods. */
        /// <summary>
        /// Get the node value representation of the object.
        /// </summary>
        protected abstract BitmaskValue ToBitmask(T obj);

        /// <summary>
        /// Parse a node value into an object.
        /// </summary>
        protected abstract T FromBitmask(BitmaskValue value);
    }
}