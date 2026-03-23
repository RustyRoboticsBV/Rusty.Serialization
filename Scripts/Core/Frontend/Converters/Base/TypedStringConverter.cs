using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converters that convert between a specific type and a string node.
    /// </summary>
    public abstract class TypedStringConverter<T> : TypedConverter<T, StringNode>
    {
        /* Public methods. */
        protected sealed override StringNode CreateNode2(T obj, CreateNodeContext context)
        {
            return new StringNode(ToString(obj));
        }

        protected sealed override T CreateObject2(StringNode node, CreateObjectContext context)
        {
            return FromString(node.Value);
        }

        /* Protected methods. */
        /// <summary>
        /// Get the string representation of the object.
        /// </summary>
        protected virtual string ToString(T obj)
        {
            return obj.ToString();
        }

        /// <summary>
        /// Parse a string into an object.
        /// </summary>
        protected abstract T FromString(string str);
    }
}