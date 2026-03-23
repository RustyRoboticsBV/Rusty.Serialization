using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converters that convert between a specific type and an char node.
    /// </summary>
    public abstract class TypedCharConverter<T> : TypedConverter<T, CharNode>
    {
        /* Public methods. */
        protected sealed override CharNode CreateNode2(T obj, CreateNodeContext context) => new CharNode(ToChar(obj));
        protected sealed override T CreateObject2(CharNode node, CreateObjectContext context) => FromChar(node.Value);

        /* Protected methods. */
        /// <summary>
        /// Get the node value representation of the object.
        /// </summary>
        protected abstract UnicodePair ToChar(T obj);

        /// <summary>
        /// Parse a node value into an object.
        /// </summary>
        protected abstract T FromChar(UnicodePair value);
    }
}