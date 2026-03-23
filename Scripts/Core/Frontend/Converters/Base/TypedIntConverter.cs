using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converters that convert between a specific type and an int node.
    /// </summary>
    public abstract class TypedIntConverter<T> : TypedConverter<T, IntNode>
    {
        /* Public methods. */
        protected sealed override IntNode CreateNode2(T obj, CreateNodeContext context) => new IntNode(ToInt(obj));
        protected sealed override T CreateObject2(IntNode node, CreateObjectContext context) => FromInt(node.Value);

        /* Protected methods. */
        /// <summary>
        /// Get the node value representation of the object.
        /// </summary>
        protected abstract IntValue ToInt(T obj);

        /// <summary>
        /// Parse a node value into an object.
        /// </summary>
        protected abstract T FromInt(IntValue value);
    }
}