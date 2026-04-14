using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic string converter.
    /// </summary>
    public class StringConverter<T> : TypedConverter<StringValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new StringNode(ToString((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(StringNode node, CreateObjectContext context) => FromString(node.Value);

        protected sealed override object PopulateObject(StringNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an string object to the internal value representation.
        /// </summary>
        protected virtual StringValue ToString(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an string object of the desired type.
        /// </summary>
        protected virtual T FromString(StringValue obj) => FromValue(obj);
    }
}