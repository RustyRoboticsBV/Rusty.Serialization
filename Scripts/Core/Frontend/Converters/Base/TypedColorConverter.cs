using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converters that convert between a specific type and a color node.
    /// </summary>
    public abstract class TypedColorConverter<T> : TypedConverter<T, ColorNode>
    {
        /* Public methods. */
        protected sealed override ColorNode CreateNode2(T obj, CreateNodeContext context) => new ColorNode(ToColor(obj));
        protected sealed override T CreateObject2(ColorNode node, CreateObjectContext context) => FromColor(node.Value);

        /* Protected methods. */
        /// <summary>
        /// Get the node value representation of the object.
        /// </summary>
        protected abstract ColorValue ToColor(T obj);

        /// <summary>
        /// Parse a node value into an object.
        /// </summary>
        protected abstract T FromColor(ColorValue value);
    }
}